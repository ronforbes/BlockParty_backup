var Tween = (function () {
    function Tween() {
    }
    Tween.Linear = /// <summary>
    /// Easing equation function for a simple linear tweening, with no easing.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change * time / duration + begin;
    };

    Tween.ExponentialEaseOut = /// <summary>
    /// Easing equation function for an exponential (2^time) easing out:
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return (time == duration) ? begin + change : change * (-Math.pow(2, -10 * time / duration) + 1) + begin;
    };

    Tween.ExponentialEaseIn = /// <summary>
    /// Easing equation function for an exponential (2^time) easing in:
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return (time == 0) ? begin : change * Math.pow(2, 10 * (time / duration - 1)) + begin;
    };

    Tween.ExponentialEaseInOut = /// <summary>
    /// Easing equation function for an exponential (2^time) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time == 0)
            return begin;

        if (time == duration)
            return begin + change;

        if ((time /= duration / 2) < 1)
            return change / 2 * Math.pow(2, 10 * (time - 1)) + begin;

        return change / 2 * (-Math.pow(2, -10 * --time) + 2) + begin;
    };

    Tween.ExponentialEaseOutIn = /// <summary>
    /// Easing equation function for an exponential (2^time) easing out/in:
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time < duration / 2)
            return this.ExponentialEaseOut(time * 2, begin, change / 2, duration);

        return this.ExponentialEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    };

    Tween.CircularEaseOut = /// <summary>
    /// Easing equation function for a circular (sqrt(1-time^2)) easing out:
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change * Math.sqrt(1 - (time = time / duration - 1) * time) + begin;
    };

    Tween.CircularEaseIn = /// <summary>
    /// Easing equation function for a circular (sqrt(1-time^2)) easing in:
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return -change * (Math.sqrt(1 - (time /= duration) * time) - 1) + begin;
    };

    Tween.CircularEaseInOut = /// <summary>
    /// Easing equation function for a circular (sqrt(1-time^2)) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if ((time /= duration / 2) < 1)
            return -change / 2 * (Math.sqrt(1 - time * time) - 1) + begin;

        return change / 2 * (Math.sqrt(1 - (time -= 2) * time) + 1) + begin;
    };

    Tween.CircularEaseOutIn = /// <summary>
    /// Easing equation function for a circular (sqrt(1-time^2)) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time < duration / 2)
            return this.CircularEaseOut(time * 2, begin, change / 2, duration);

        return this.CircularEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    };

    Tween.QuadraticEaseOut = /// <summary>
    /// Easing equation function for a quadratic (time^2) easing out:
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return -change * (time /= duration) * (time - 2) + begin;
    };

    Tween.QuadraticEaseIn = /// <summary>
    /// Easing equation function for a quadratic (time^2) easing in:
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change * (time /= duration) * time + begin;
    };

    Tween.QuadraticEaseInOut = /// <summary>
    /// Easing equation function for a quadratic (time^2) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if ((time /= duration / 2) < 1)
            return change / 2 * time * time + begin;

        return -change / 2 * ((--time) * (time - 2) - 1) + begin;
    };

    Tween.QuadraticEaseOutIn = /// <summary>
    /// Easing equation function for a quadratic (time^2) easing out/in:
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time < duration / 2)
            return this.QuadraticEaseOut(time * 2, begin, change / 2, duration);

        return this.QuadraticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    };

    Tween.SineEaseOut = /// <summary>
    /// Easing equation function for a sinusoidal (sin(time)) easing out:
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change * Math.sin(time / duration * (Math.PI / 2)) + begin;
    };

    Tween.SineEaseIn = /// <summary>
    /// Easing equation function for a sinusoidal (sin(time)) easing in:
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return -change * Math.cos(time / duration * (Math.PI / 2)) + change + begin;
    };

    Tween.SineEaseInOut = /// <summary>
    /// Easing equation function for a sinusoidal (sin(time)) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if ((time /= duration / 2) < 1)
            return change / 2 * (Math.sin(Math.PI * time / 2)) + begin;

        return -change / 2 * (Math.cos(Math.PI * --time / 2) - 2) + begin;
    };

    Tween.SineEaseOutIn = /// <summary>
    /// Easing equation function for a sinusoidal (sin(time)) easing in/out:
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time < duration / 2)
            return this.SineEaseOut(time * 2, begin, change / 2, duration);

        return this.SineEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    };

    Tween.CubicEaseOut = /// <summary>
    /// Easing equation function for a cubic (time^3) easing out:
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change * ((time = time / duration - 1) * time * time + 1) + begin;
    };

    Tween.CubicEaseIn = /// <summary>
    /// Easing equation function for a cubic (time^3) easing in:
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change * (time /= duration) * time * time + begin;
    };

    Tween.CubicEaseInOut = /// <summary>
    /// Easing equation function for a cubic (time^3) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if ((time /= duration / 2) < 1)
            return change / 2 * time * time * time + begin;

        return change / 2 * ((time -= 2) * time * time + 2) + begin;
    };

    Tween.CubicEaseOutIn = /// <summary>
    /// Easing equation function for a cubic (time^3) easing out/in:
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time < duration / 2)
            return this.CubicEaseOut(time * 2, begin, change / 2, duration);

        return this.CubicEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    };

    Tween.QuarticEaseOut = /// <summary>
    /// Easing equation function for a quartic (time^4) easing out:
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return -change * ((time = time / duration - 1) * time * time * time - 1) + begin;
    };

    Tween.QuarticEaseIn = /// <summary>
    /// Easing equation function for a quartic (time^4) easing in:
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change * (time /= duration) * time * time * time + begin;
    };

    Tween.QuarticEaseInOut = /// <summary>
    /// Easing equation function for a quartic (time^4) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if ((time /= duration / 2) < 1)
            return change / 2 * time * time * time * time + begin;

        return -change / 2 * ((time -= 2) * time * time * time - 2) + begin;
    };

    Tween.QuarticEaseOutIn = /// <summary>
    /// Easing equation function for a quartic (time^4) easing out/in:
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time < duration / 2)
            return this.QuarticEaseOut(time * 2, begin, change / 2, duration);

        return this.QuarticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    };

    Tween.QuinticEaseOut = /// <summary>
    /// Easing equation function for a quintic (time^5) easing out:
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change * ((time = time / duration - 1) * time * time * time * time + 1) + begin;
    };

    Tween.QuinticEaseIn = /// <summary>
    /// Easing equation function for a quintic (time^5) easing in:
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change * (time /= duration) * time * time * time * time + begin;
    };

    Tween.QuinticEaseInOut = /// <summary>
    /// Easing equation function for a quintic (time^5) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if ((time /= duration / 2) < 1)
            return change / 2 * time * time * time * time * time + begin;
        return change / 2 * ((time -= 2) * time * time * time * time + 2) + begin;
    };

    Tween.QuinticEaseOutIn = /// <summary>
    /// Easing equation function for a quintic (time^5) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time < duration / 2)
            return this.QuinticEaseOut(time * 2, begin, change / 2, duration);
        return this.QuinticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    };

    Tween.ElasticEaseOut = /// <summary>
    /// Easing equation function for an elastic (exponentially decaying sine wave) easing out:
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if ((time /= duration) == 1)
            return begin + change;

        var p = duration * .3;
        var s = p / 4;

        return (change * Math.pow(2, -10 * time) * Math.sin((time * duration - s) * (2 * Math.PI) / p) + change + begin);
    };

    Tween.ElasticEaseIn = /// <summary>
    /// Easing equation function for an elastic (exponentially decaying sine wave) easing in:
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if ((time /= duration) == 1)
            return begin + change;

        var p = duration * .3;
        var s = p / 4;

        return -(change * Math.pow(2, 10 * (time -= 1)) * Math.sin((time * duration - s) * (2 * Math.PI) / p)) + begin;
    };

    Tween.ElasticEaseInOut = /// <summary>
    /// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if ((time /= duration / 2) == 2)
            return begin + change;

        var p = duration * (.3 * 1.5);
        var s = p / 4;

        if (time < 1)
            return -.5 * (change * Math.pow(2, 10 * (time -= 1)) * Math.sin((time * duration - s) * (2 * Math.PI) / p)) + begin;
        return change * Math.pow(2, -10 * (time -= 1)) * Math.sin((time * duration - s) * (2 * Math.PI) / p) * .5 + change + begin;
    };

    Tween.ElasticEaseOutIn = /// <summary>
    /// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in:
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time < duration / 2)
            return this.ElasticEaseOut(time * 2, begin, change / 2, duration);
        return this.ElasticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    };

    Tween.BounceEaseOut = /// <summary>
    /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out:
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if ((time /= duration) < (1 / 2.75))
            return change * (7.5625 * time * time) + begin;
else if (time < (2 / 2.75))
            return change * (7.5625 * (time -= (1.5 / 2.75)) * time + .75) + begin;
else if (time < (2.5 / 2.75))
            return change * (7.5625 * (time -= (2.25 / 2.75)) * time + .9375) + begin;
else
            return change * (7.5625 * (time -= (2.625 / 2.75)) * time + .984375) + begin;
    };

    Tween.BounceEaseIn = /// <summary>
    /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in:
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change - this.BounceEaseOut(duration - time, 0, change, duration) + begin;
    };

    Tween.BounceEaseInOut = /// <summary>
    /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time < duration / 2)
            return this.BounceEaseIn(time * 2, 0, change, duration) * .5 + begin;
else
            return this.BounceEaseOut(time * 2 - duration, 0, change, duration) * .5 + change * .5 + begin;
    };

    Tween.BounceEaseOutIn = /// <summary>
    /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in:
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time < duration / 2)
            return this.BounceEaseOut(time * 2, begin, change / 2, duration);
        return this.BounceEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    };

    Tween.BackEaseOut = /// <summary>
    /// Easing equation function for a back (overshooting cubic easing: (s+1)*time^3 - s*time^2) easing out:
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change * ((time = time / duration - 1) * time * ((1.70158 + 1) * time + 1.70158) + 1) + begin;
    };

    Tween.BackEaseIn = /// <summary>
    /// Easing equation function for a back (overshooting cubic easing: (s+1)*time^3 - s*time^2) easing in:
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        return change * (time /= duration) * time * ((1.70158 + 1) * time - 1.70158) + begin;
    };

    Tween.BackEaseInOut = /// <summary>
    /// Easing equation function for a back (overshooting cubic easing: (s+1)*time^3 - s*time^2) easing in/out:
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        var s = 1.70158;
        if ((time /= duration / 2) < 1)
            return change / 2 * (time * time * (((s *= (1.525)) + 1) * time - s)) + begin;
        return change / 2 * ((time -= 2) * time * (((s *= (1.525)) + 1) * time + s) + 2) + begin;
    };

    Tween.BackEaseOutIn = /// <summary>
    /// Easing equation function for a back (overshooting cubic easing: (s+1)*time^3 - s*time^2) easing out/in:
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    function (time, begin, change, duration) {
        if (time < duration / 2)
            return this.BackEaseOut(time * 2, begin, change / 2, duration);
        return this.BackEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    };
    return Tween;
})();
