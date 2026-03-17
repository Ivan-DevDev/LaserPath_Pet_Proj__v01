namespace Assets.LazerPath2D.Scripts.CommonUI.Presenter
{
    public interface ISubscribedPresenter: IPresenter
    {
        void Subscribe();

        void Unsubscribe();
    }
}
