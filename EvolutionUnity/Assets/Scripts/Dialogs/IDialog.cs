public interface IDialog {
    void Create(DialogTypes type);
    void Show();
    void Hide();
    void Destroy();
}
