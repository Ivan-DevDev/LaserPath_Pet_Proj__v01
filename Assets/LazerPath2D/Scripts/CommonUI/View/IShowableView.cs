using DG.Tweening;

namespace Assets.LazerPath2D.Scripts.CommonUI.View
{
    public interface IShowableView : IView
    {
        Tween Hide();

        Tween Show();
    }
}
