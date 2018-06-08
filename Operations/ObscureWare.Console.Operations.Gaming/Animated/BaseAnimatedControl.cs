namespace ObscureWare.Console.Operations.Controls.Animated
{
    using System;
    using System.Drawing;
    using System.Linq;

    using ObscureWare.Console.Root.Shared;
    using ObscureWare.Console.Shared;

    public abstract class BaseAnimatedControl
    {
        private readonly IMasterAnimator _animator;

        public Rectangle OccupiedArea { get; set; }

        protected BaseAnimatedControl(IMasterAnimator animator)
        {
            _animator = animator;
        }

        public void Render(IAtomicConsole console)
        {
            // TODO: check if animation frame time has passed for this animation, if so - animate!
            // there might be various types of animators - changing with frames, seconds, different speeds or only after change occurred (some progress bars)

            // TODO: also, check whether animation is not out of the screen / buffer - it's kind of stupid to render something offscreen
            // TODO: track indirect positions in buffer as well and transform positions? Another event?

            this.OnRenderFrame(console);
        }

        protected abstract void OnRenderFrame(IAtomicConsole console);
    }

    public class EnlightedLabel : BaseAnimatedControl
    {
        public string DisplayedText { get; set; }

        public EnlightedLabel(IMasterAnimator animator, Color baseColor, int phazes, Color bgColor) : base(animator)
        {
            if (phazes <= 3 || phazes > 100) throw new ArgumentOutOfRangeException(nameof(phazes));

            //_baseColors = this.CalculatePhazes(bgColor, baseColor, phazes);
        }

        private object CalculatePhazes(Color bgColor, Color baseColor, int phazes)
        {
            throw new NotImplementedException();
        }

        protected override void OnRenderFrame(IAtomicConsole console)
        {
            var lines = (DisplayedText ?? "")
                .SplitTextToFit((uint)OccupiedArea.Width) // do not overflow horizontally
                .Take(OccupiedArea.Height);               // do not overflow vertically


        }
    }

    public interface IMasterAnimator

    {
    }
}
