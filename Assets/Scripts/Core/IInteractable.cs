namespace Core
{
    public interface IInteractable
    {
        public void Focus();
        public void Unfocus();
        public void Select();
        public void Deselect();
    }
}
