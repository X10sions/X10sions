namespace Common.Html.Css.Enums {
  public enum AnimationTimingFunction {
    // https://www.w3schools.com/cssref/css3_pr_animation-timing-function.asp
    ease,
    easeIn,
    easeOut,
    easeInOut,
    linear
  }

  public enum AnimationEasing {
    // AnimationTimingFunction
    Quad,
    Cubic,
    Quart,
    Quint,
    Sine,
    Expo,
    Circ,
    Elastic,
    Back,
    Bounce

    ///Linear,
    //EaseInQuad,
    //EaseOutQuad,
    //EaseInOutQuad,
    //EaseInCubic,
    //EaseOutCubic,
    //EaseInOutCubic,
    //EaseInQuart,
    //EaseOutQuart,
    //EaseInOutQuart,
    //EaseInQuint,
    //EaseOutQuint,
    //EaseInOutQuint,
    //EaseInSine,
    //EaseOutSine,
    //EaseInOutSine,
    //EaseInExpo,
    //EaseOutExpo,
    //EaseInOutExpo,
    //EaseInCirc,
    //EaseOutCirc,
    //EaseInOutCirc,
    //EaseInElastic,
    //EaseOutElastic,
    //EaseInOutElastic,
    //EaseInBack,
    //EaseOutBack,
    //EaseInOutBack,
    //EaseInBounce,
    //EaseOutBounce,
    //EaseInOutBounce
  }
  public static class AnimationTimingFunctionExtensions {

        public static string CubicBezier(this AnimationTimingFunction timing, AnimationEasing? easing) {
      (double x1, double y1, double x2, double y2) cb = timing switch {
        AnimationTimingFunction.linear => (0.250, 0.250, 0.750, 0.750),
        AnimationTimingFunction.ease => (0.250, 0.100, 0.250, 1.000),
        AnimationTimingFunction.easeIn => easing switch {
          AnimationEasing.Quad => (0.550, 0.085, 0.680, 0.530),
          AnimationEasing.Cubic => (0.550, 0.055, 0.675, 0.190),
          AnimationEasing.Quart => (0.895, 0.030, 0.685, 0.220),
          AnimationEasing.Quint => (0.755, 0.050, 0.855, 0.060),
          AnimationEasing.Sine => (0.470, 0.000, 0.745, 0.715),
          AnimationEasing.Expo => (0.950, 0.050, 0.795, 0.035),
          AnimationEasing.Circ => (0.600, 0.040, 0.980, 0.335),
          AnimationEasing.Back => (0.600, -0.280, 0.735, 0.045),
          _ => (0.420, 0.000, 1.000, 1.000)
        },
        AnimationTimingFunction.easeInOut => easing switch {
          AnimationEasing.Quad => (0.455, 0.030, 0.515, 0.955),
          AnimationEasing.Cubic => (0.645, 0.045, 0.355, 1.000),
          AnimationEasing.Quart => (0.770, 0.000, 0.175, 1.000),
          AnimationEasing.Quint => (0.860, 0.000, 0.070, 1.000),
          AnimationEasing.Sine => (0.445, 0.050, 0.550, 0.950),
          AnimationEasing.Expo => (1.000, 0.000, 0.000, 1.000),
          AnimationEasing.Circ => (0.785, 0.135, 0.150, 0.860),
          AnimationEasing.Back => (0.680, -0.550, 0.265, 1.550),
          _ => (0.420, 0.000, 0.580, 1.000)
        },
        AnimationTimingFunction.easeOut => easing switch {
          AnimationEasing.Quad => (0.250, 0.460, 0.450, 0.940),
          AnimationEasing.Cubic => (0.215, 0.610, 0.355, 1.000),
          AnimationEasing.Quart => (0.165, 0.840, 0.440, 1.000),
          AnimationEasing.Quint => (0.230, 1.000, 0.320, 1.000),
          AnimationEasing.Sine => (0.390, 0.575, 0.565, 1.000),
          AnimationEasing.Expo => (0.190, 1.000, 0.220, 1.000),
          AnimationEasing.Circ => (0.075, 0.820, 0.165, 1.000),
          AnimationEasing.Back => (0.175, 0.885, 0.320, 1.275),
          _ => (0.000, 0.000, 0.580, 1.000)
        },
        _ => throw new System.NotImplementedException()
      };
      return Functions.CubicBezier(cb.x1, cb.y1, cb.x2, cb.y2);
    }

  }
}