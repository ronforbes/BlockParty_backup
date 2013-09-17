class Tween {
    /// <summary>
    /// Easing equation function for a simple linear tweening, with no easing.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static Linear(time: number, begin: number, change: number, duration: number): number {
        return change * time / duration + begin;
    }

    /// <summary>
    /// Easing equation function for an exponential (2^time) easing out: 
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static ExponentialEaseOut(time: number, begin: number, change: number, duration: number): number {
        return (time == duration) ? begin + change : change * (-Math.pow(2, -10 * time / duration) + 1) + begin;
    }

    /// <summary>
    /// Easing equation function for an exponential (2^time) easing in: 
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static ExponentialEaseIn(time: number, begin: number, change: number, duration: number): number {
        return (time == 0) ? begin : change * Math.pow(2, 10 * (time / duration - 1)) + begin;
    }

    /// <summary>
    /// Easing equation function for an exponential (2^time) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static ExponentialEaseInOut(time: number, begin: number, change: number, duration: number): number {
        if (time == 0)
            return begin;

        if (time == duration)
            return begin + change;

        if ((time /= duration / 2) < 1)
            return change / 2 * Math.pow(2, 10 * (time - 1)) + begin;

        return change / 2 * (-Math.pow(2, -10 * --time) + 2) + begin;
    }

    /// <summary>
    /// Easing equation function for an exponential (2^time) easing out/in: 
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static ExponentialEaseOutIn(time: number, begin: number, change: number, duration: number): number {
        if (time < duration / 2)
            return this.ExponentialEaseOut(time * 2, begin, change / 2, duration);

        return this.ExponentialEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    }

    /// <summary>
    /// Easing equation function for a circular (sqrt(1-time^2)) easing out: 
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static CircularEaseOut(time: number, begin: number, change: number, duration: number): number {
        return change * Math.sqrt(1 - (time = time / duration - 1) * time) + begin;
    }

    /// <summary>
    /// Easing equation function for a circular (sqrt(1-time^2)) easing in: 
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static CircularEaseIn(time: number, begin: number, change: number, duration: number): number {
        return -change * (Math.sqrt(1 - (time /= duration) * time) - 1) + begin;
    }

    /// <summary>
    /// Easing equation function for a circular (sqrt(1-time^2)) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static CircularEaseInOut(time: number, begin: number, change: number, duration: number): number {
        if ((time /= duration / 2) < 1)
            return -change / 2 * (Math.sqrt(1 - time * time) - 1) + begin;

        return change / 2 * (Math.sqrt(1 - (time -= 2) * time) + 1) + begin;
    }

    /// <summary>
    /// Easing equation function for a circular (sqrt(1-time^2)) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static CircularEaseOutIn(time: number, begin: number, change: number, duration: number): number {
        if (time < duration / 2)
            return this.CircularEaseOut(time * 2, begin, change / 2, duration);

        return this.CircularEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    }

    /// <summary>
    /// Easing equation function for a quadratic (time^2) easing out: 
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuadraticEaseOut(time: number, begin: number, change: number, duration: number): number {
        return -change * (time /= duration) * (time - 2) + begin;
    }

    /// <summary>
    /// Easing equation function for a quadratic (time^2) easing in: 
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuadraticEaseIn(time: number, begin: number, change: number, duration: number): number {
        return change * (time /= duration) * time + begin;
    }

    /// <summary>
    /// Easing equation function for a quadratic (time^2) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuadraticEaseInOut(time: number, begin: number, change: number, duration: number): number {
        if ((time /= duration / 2) < 1)
            return change / 2 * time * time + begin;
        
        return -change / 2 * ((--time) * (time - 2) - 1) + begin;
    }

    /// <summary>
    /// Easing equation function for a quadratic (time^2) easing out/in: 
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuadraticEaseOutIn(time: number, begin: number, change: number, duration: number): number {
        if (time < duration / 2)
            return this.QuadraticEaseOut(time * 2, begin, change / 2, duration);

        return this.QuadraticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    }

    /// <summary>
    /// Easing equation function for a sinusoidal (sin(time)) easing out: 
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static SineEaseOut(time: number, begin: number, change: number, duration: number): number {
        return change * Math.sin(time / duration * (Math.PI / 2)) + begin;
    }

    /// <summary>
    /// Easing equation function for a sinusoidal (sin(time)) easing in: 
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static SineEaseIn(time: number, begin: number, change: number, duration: number): number {
        return -change * Math.cos(time / duration * (Math.PI / 2)) + change + begin;
    }

    /// <summary>
    /// Easing equation function for a sinusoidal (sin(time)) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static SineEaseInOut(time: number, begin: number, change: number, duration: number): number {
        if ((time /= duration / 2) < 1)
            return change / 2 * (Math.sin(Math.PI * time / 2)) + begin;

        return -change / 2 * (Math.cos(Math.PI * --time / 2) - 2) + begin;
    }

    /// <summary>
    /// Easing equation function for a sinusoidal (sin(time)) easing in/out: 
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static SineEaseOutIn(time: number, begin: number, change: number, duration: number): number {
        if (time < duration / 2)
            return this.SineEaseOut(time * 2, begin, change / 2, duration);

        return this.SineEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    }

    /// <summary>
    /// Easing equation function for a cubic (time^3) easing out: 
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static CubicEaseOut(time: number, begin: number, change: number, duration: number): number {
        return change * ((time = time / duration - 1) * time * time + 1) + begin;
    }

    /// <summary>
    /// Easing equation function for a cubic (time^3) easing in: 
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static CubicEaseIn(time: number, begin: number, change: number, duration: number): number {
        return change * (time /= duration) * time * time + begin;
    }

    /// <summary>
    /// Easing equation function for a cubic (time^3) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static CubicEaseInOut(time: number, begin: number, change: number, duration: number): number {
        if ((time /= duration / 2) < 1)
            return change / 2 * time * time * time + begin;

        return change / 2 * ((time -= 2) * time * time + 2) + begin;
    }

    /// <summary>
    /// Easing equation function for a cubic (time^3) easing out/in: 
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static CubicEaseOutIn(time: number, begin: number, change: number, duration: number): number {
        if (time < duration / 2)
            return this.CubicEaseOut(time * 2, begin, change / 2, duration);

        return this.CubicEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    }

    /// <summary>
    /// Easing equation function for a quartic (time^4) easing out: 
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuarticEaseOut(time: number, begin: number, change: number, duration: number): number {
        return -change * ((time = time / duration - 1) * time * time * time - 1) + begin;
    }

    /// <summary>
    /// Easing equation function for a quartic (time^4) easing in: 
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuarticEaseIn(time: number, begin: number, change: number, duration: number): number {
        return change * (time /= duration) * time * time * time + begin;
    }

    /// <summary>
    /// Easing equation function for a quartic (time^4) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuarticEaseInOut(time: number, begin: number, change: number, duration: number): number {
        if ((time /= duration / 2) < 1)
            return change / 2 * time * time * time * time + begin;

        return -change / 2 * ((time -= 2) * time * time * time - 2) + begin;
    }

    /// <summary>
    /// Easing equation function for a quartic (time^4) easing out/in: 
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuarticEaseOutIn(time: number, begin: number, change: number, duration: number): number {
        if (time < duration / 2)
            return this.QuarticEaseOut(time * 2, begin, change / 2, duration);

        return this.QuarticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    }

    /// <summary>
    /// Easing equation function for a quintic (time^5) easing out: 
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuinticEaseOut(time: number, begin: number, change: number, duration: number): number {
        return change * ((time = time / duration - 1) * time * time * time * time + 1) + begin;
    }

    /// <summary>
    /// Easing equation function for a quintic (time^5) easing in: 
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuinticEaseIn(time: number, begin: number, change: number, duration: number): number {
        return change * (time /= duration) * time * time * time * time + begin;
    }

    /// <summary>
    /// Easing equation function for a quintic (time^5) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuinticEaseInOut(time: number, begin: number, change: number, duration: number): number {
        if ((time /= duration / 2) < 1)
            return change / 2 * time * time * time * time * time + begin;
        return change / 2 * ((time -= 2) * time * time * time * time + 2) + begin;
    }

    /// <summary>
    /// Easing equation function for a quintic (time^5) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static QuinticEaseOutIn(time: number, begin: number, change: number, duration: number): number {
        if (time < duration / 2)
            return this.QuinticEaseOut(time * 2, begin, change / 2, duration);
        return this.QuinticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    }

    /// <summary>
    /// Easing equation function for an elastic (exponentially decaying sine wave) easing out: 
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static ElasticEaseOut(time: number, begin: number, change: number, duration: number): number {
        if ((time /= duration) == 1)
            return begin + change;

        var p: number = duration * .3;
        var s: number = p / 4;

        return (change * Math.pow(2, -10 * time) * Math.sin((time * duration - s) * (2 * Math.PI) / p) + change + begin);
    }

    /// <summary>
    /// Easing equation function for an elastic (exponentially decaying sine wave) easing in: 
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static ElasticEaseIn(time: number, begin: number, change: number, duration: number): number {
        if ((time /= duration) == 1)
            return begin + change;

        var p: number = duration * .3;
        var s: number = p / 4;

        return -(change * Math.pow(2, 10 * (time -= 1)) * Math.sin((time * duration - s) * (2 * Math.PI) / p)) + begin;
    }

    /// <summary>
    /// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static ElasticEaseInOut(time: number, begin: number, change: number, duration: number): number {
        if ((time /= duration / 2) == 2)
            return begin + change;

        var p: number = duration * (.3 * 1.5);
        var s: number = p / 4;

        if (time < 1)
            return -.5 * (change * Math.pow(2, 10 * (time -= 1)) * Math.sin((time * duration - s) * (2 * Math.PI) / p)) + begin;
        return change * Math.pow(2, -10 * (time -= 1)) * Math.sin((time * duration - s) * (2 * Math.PI) / p) * .5 + change + begin;
    }

    /// <summary>
    /// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in: 
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static ElasticEaseOutIn(time: number, begin: number, change: number, duration: number): number {
        if (time < duration / 2)
            return this.ElasticEaseOut(time * 2, begin, change / 2, duration);
        return this.ElasticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    }

    /// <summary>
    /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: 
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static BounceEaseOut(time: number, begin: number, change: number, duration: number): number {
        if ((time /= duration) < (1 / 2.75))
            return change * (7.5625 * time * time) + begin;
        else if (time < (2 / 2.75))
            return change * (7.5625 * (time -= (1.5 / 2.75)) * time + .75) + begin;
        else if (time < (2.5 / 2.75))
            return change * (7.5625 * (time -= (2.25 / 2.75)) * time + .9375) + begin;
        else
            return change * (7.5625 * (time -= (2.625 / 2.75)) * time + .984375) + begin;
    }

    /// <summary>
    /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: 
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static BounceEaseIn(time: number, begin: number, change: number, duration: number): number {
        return change - this.BounceEaseOut(duration - time, 0, change, duration) + begin;
    }

    /// <summary>
    /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static BounceEaseInOut(time: number, begin: number, change: number, duration: number): number {
        if (time < duration / 2)
            return this.BounceEaseIn(time * 2, 0, change, duration) * .5 + begin;
        else
            return this.BounceEaseOut(time * 2 - duration, 0, change, duration) * .5 + change * .5 + begin;
    }

    /// <summary>
    /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in: 
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static BounceEaseOutIn(time: number, begin: number, change: number, duration: number): number {
        if (time < duration / 2)
            return this.BounceEaseOut(time * 2, begin, change / 2, duration);
        return this.BounceEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    }

    /// <summary>
    /// Easing equation function for a back (overshooting cubic easing: (s+1)*time^3 - s*time^2) easing out: 
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static BackEaseOut(time: number, begin: number, change: number, duration: number): number {
        return change * ((time = time / duration - 1) * time * ((1.70158 + 1) * time + 1.70158) + 1) + begin;
    }

    /// <summary>
    /// Easing equation function for a back (overshooting cubic easing: (s+1)*time^3 - s*time^2) easing in: 
    /// accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static BackEaseIn(time: number, begin: number, change: number, duration: number): number {
        return change * (time /= duration) * time * ((1.70158 + 1) * time - 1.70158) + begin;
    }

    /// <summary>
    /// Easing equation function for a back (overshooting cubic easing: (s+1)*time^3 - s*time^2) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static BackEaseInOut(time: number, begin: number, change: number, duration: number): number {
        var s: number = 1.70158;
        if ((time /= duration / 2) < 1)
            return change / 2 * (time * time * (((s *= (1.525)) + 1) * time - s)) + begin;
        return change / 2 * ((time -= 2) * time * (((s *= (1.525)) + 1) * time + s) + 2) + begin;
    }

    /// <summary>
    /// Easing equation function for a back (overshooting cubic easing: (s+1)*time^3 - s*time^2) easing out/in: 
    /// deceleration until halfway, then acceleration.
    /// </summary>
    /// <param name="time">Current time in seconds.</param>
    /// <param name="begin">Starting value.</param>
    /// <param name="change">Final value.</param>
    /// <param name="duration">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static BackEaseOutIn(time: number, begin: number, change: number, duration: number): number {
        if (time < duration / 2)
            return this.BackEaseOut(time * 2, begin, change / 2, duration);
        return this.BackEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
    }
}