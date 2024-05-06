using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using NnUtils.Scripts;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items.Plants
{
	public class PlantScript : MonoBehaviour, IInteractable
	{
		public PlantSO PlantSO;
		[SerializeField] private Transform _meshTransform;
		[SerializeField] private AnimationCurve _selectAnimCurve, _hitAnimCurve;
		[HideInInspector] public int CurrentHealth;
		[HideInInspector] public bool IsFertilized;
		private bool IsSelected;

		[Header("Particles")]
		[SerializeField] private List<ParticleSystem> _placementParticles;
		[SerializeField] private List<ParticleSystem> _maturedParticles;
		[SerializeField] private List<ParticleSystem> _hitParticles;
		[SerializeField] private List<ParticleSystem> _harvestParticles;

		private float _maturity;
		public float Maturity
		{
			get => _maturity;
			private set
			{
				_maturity = value;
				OnMaturityChanged?.Invoke();
			}
		}
		public Action OnMaturityChanged, OnMatured;
		
		#region IInteractable 
		public void Focus()
		{
			
		}

		public void Unfocus()
		{
			
		}

		public void Select()
		{
			if (IsSelected)
			{
				GameManager.Instance.SelectionManager.SelectedObject = null;
				return;
			}

			GameManager.Instance.UIManager.ShowInfo(this);
			IsSelected = true;
			
			if (_animationRoutine != null)
			{
				StopCoroutine(_animationRoutine);
				_animationRoutine = null;
			}
			Animate(Vector3.zero, Vector3.zero, Vector3.one * Random.Range(0.05f, 0.1f), 0.5f, _selectAnimCurve);
		}

		public void Deselect()
		{
			var sm = GameManager.Instance.SelectionManager;
			if ((transform.parent.gameObject == sm.ToBeSelected && gameObject == sm.SelectedObject) ||
			    gameObject == sm.ToBeSelected) return;
			GameManager.Instance.UIManager.HideInfo();
			IsSelected = false;
		}
		#endregion

		private void Start() => Plant();

		#region Plant 
		private void Plant()
		{
			transform.localScale = Vector3.zero;
			StartCoroutine(PlantRoutine());
		}

		private IEnumerator PlantRoutine()
		{
			foreach (var particle in _placementParticles) particle.Play();
            GameManager.Instance.AudioManager.PlaySFXOnObject(AudioManager.Sounds.PlantPlace, gameObject, 0.2f);
			float lerpPos = 0;
			transform.localRotation = quaternion.Euler(Vector3.up * Random.Range(-360, 360));
			var size = Vector3.one * Random.Range(0.75f, 1.25f);
			while (lerpPos < 1)
			{
				var t = PlantSO.GrowthCurve.Evaluate(Misc.UpdateLerpPos(ref lerpPos, PlantSO.GrowthTime));
				transform.localScale = size * t;
				Maturity = lerpPos;
				yield return null;
			}
			foreach (var particle in _maturedParticles) particle.Play();
			OnMatured?.Invoke();
		}
		#endregion
		#region Harvest
		public void Harvest()
		{
			if (_harvestRoutine != null) return;
			_harvestRoutine = StartCoroutine(HarvestRoutine());
		}

		private Coroutine _harvestRoutine;
		private IEnumerator HarvestRoutine()
		{
			for (var repeat = Random.Range(3, 8); repeat >= 0; repeat--)
			{
				foreach (var particle in _hitParticles)
				{
					var hp = Instantiate(particle, particle.transform.position, particle.transform.rotation);
					hp.Play();
					Destroy(hp.gameObject, 10);
				}
				
				GameManager.Instance.AudioManager.PlaySFXAtPosition(AudioManager.Sounds.TreeHit, transform.position, 0.1f);

				Animate(
					Vector3.one * Random.Range(-0.3f, 0.3f),
					Vector3.up * Random.Range(-15f, 15f),
					Vector3.one * Random.Range(-0.1f, 0.1f),
					0.5f, _hitAnimCurve);
				if (repeat != 0) yield return new WaitForSeconds(Random.Range(2f, 5f));
			}
			
			foreach (var particle in _harvestParticles)
			{
				var hp = Instantiate(particle, transform.position, transform.rotation);
				hp.Play();
				Destroy(hp.gameObject, 10);
			}

			_harvestRoutine = null;
			GameManager.Instance.SelectionManager.SelectedObject = null;
			GameManager.Instance.PlanetManagerScript.CurrentPlanet.RemoveItem(PlantSO, gameObject);
		}
#endregion
#region Animation 
		private void Animate(Vector3 posDelta, Vector3 rotDelta, Vector3 scaleDelta, float time, AnimationCurve curve) 
		{
			Misc.RestartCoroutine(this, ref _animationRoutine, 
				AnimationRoutine(posDelta, Quaternion.Euler(rotDelta), Vector3.one + scaleDelta, time, curve));
		}
		private Coroutine _animationRoutine;
		private IEnumerator AnimationRoutine(Vector3 targetPos, Quaternion targetRot, Vector3 targetScale, float time, AnimationCurve curve)
		{
			float lerpPos = 0;
			var startRot = Quaternion.Euler(Vector3.zero);
			while (lerpPos < 1)
			{
				var t = curve.Evaluate(Misc.UpdateLerpPos(ref lerpPos, time));
				_meshTransform.localPosition = Vector3.LerpUnclamped(Vector3.zero, targetPos, t);
				_meshTransform.localRotation = Quaternion.LerpUnclamped(startRot, targetRot, t);
				_meshTransform.localScale = Vector3.LerpUnclamped(Vector3.one, targetScale, t);
				yield return null;
			}
			_animationRoutine = null;
		}
#endregion
	}
}
