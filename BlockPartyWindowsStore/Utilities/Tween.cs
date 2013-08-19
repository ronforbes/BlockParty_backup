using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Globalization;

namespace BlockPartyWindowsStore
{
    public class Tween
    {
        #region Linear

        /// <summary>
        /// Easing equation function for a simple linear tweening, with no easing.
        /// </summary>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="begin">Starting value.</param>
        /// <param name="change">Final value.</param>
        /// <param name="duration">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double Linear(double time, double begin, double change, double duration)
        {
            return change * time / duration + begin;
        }

        #endregion

        #region Exponential

        /// <summary>
        /// Easing equation function for an exponential (2^time) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="begin">Starting value.</param>
        /// <param name="change">Final value.</param>
        /// <param name="duration">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ExponentialEaseOut(double time, double begin, double change, double duration)
        {
            return (time == duration) ? begin + change : change * (-Math.Pow(2, -10 * time / duration) + 1) + begin;
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
        public static double ExponentialEaseIn(double time, double begin, double change, double duration)
        {
            return (time == 0) ? begin : change * Math.Pow(2, 10 * (time / duration - 1)) + begin;
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
        public static double ExponentialEaseInOut(double time, double begin, double change, double duration)
        {
            if (time == 0)
                return begin;

            if (time == duration)
                return begin + change;

            if ((time /= duration / 2) < 1)
                return change / 2 * Math.Pow(2, 10 * (time - 1)) + begin;

            return change / 2 * (-Math.Pow(2, -10 * --time) + 2) + begin;
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
        public static double ExponentialEaseOutIn(double time, double begin, double change, double duration)
        {
            if (time < duration / 2)
                return ExponentialEaseOut(time * 2, begin, change / 2, duration);

            return ExponentialEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
        }

        #endregion

        #region Circular

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-time^2)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="begin">Starting value.</param>
        /// <param name="change">Final value.</param>
        /// <param name="duration">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CircularEaseOut(double time, double begin, double change, double duration)
        {
            return change * Math.Sqrt(1 - (time = time / duration - 1) * time) + begin;
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
        public static double CircularEaseIn(double time, double begin, double change, double duration)
        {
            return -change * (Math.Sqrt(1 - (time /= duration) * time) - 1) + begin;
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
        public static double CircularEaseInOut(double time, double begin, double change, double duration)
        {
            if ((time /= duration / 2) < 1)
                return -change / 2 * (Math.Sqrt(1 - time * time) - 1) + begin;

            return change / 2 * (Math.Sqrt(1 - (time -= 2) * time) + 1) + begin;
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
        public static double CircularEaseOutIn(double time, double begin, double change, double duration)
        {
            if (time < duration / 2)
                return CircularEaseOut(time * 2, begin, change / 2, duration);

            return CircularEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
        }

        #endregion

        #region Quadratic

        /// <summary>
        /// Easing equation function for a quadratic (time^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="begin">Starting value.</param>
        /// <param name="change">Final value.</param>
        /// <param name="duration">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuadraticEaseOut(double time, double begin, double change, double duration)
        {
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
        public static double QuadraticEaseIn(double time, double begin, double change, double duration)
        {
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
        public static double QuadraticEaseInOut(double time, double begin, double change, double duration)
        {
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
        public static double QuadEaseOutIn(double time, double begin, double change, double duration)
        {
            if (time < duration / 2)
                return QuadraticEaseOut(time * 2, begin, change / 2, duration);

            return QuadraticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
        }

        #endregion

        #region Sine

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(time)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="begin">Starting value.</param>
        /// <param name="change">Final value.</param>
        /// <param name="duration">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double SineEaseOut(double time, double begin, double change, double duration)
        {
            return change * Math.Sin(time / duration * (Math.PI / 2)) + begin;
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
        public static double SineEaseIn(double time, double begin, double change, double duration)
        {
            return -change * Math.Cos(time / duration * (Math.PI / 2)) + change + begin;
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
        public static double SineEaseInOut(double time, double begin, double change, double duration)
        {
            if ((time /= duration / 2) < 1)
                return change / 2 * (Math.Sin(Math.PI * time / 2)) + begin;

            return -change / 2 * (Math.Cos(Math.PI * --time / 2) - 2) + begin;
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
        public static double SineEaseOutIn(double time, double begin, double change, double duration)
        {
            if (time < duration / 2)
                return SineEaseOut(time * 2, begin, change / 2, duration);

            return SineEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
        }

        #endregion

        #region Cubic

        /// <summary>
        /// Easing equation function for a cubic (time^3) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="begin">Starting value.</param>
        /// <param name="change">Final value.</param>
        /// <param name="duration">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double CubicEaseOut(double time, double begin, double change, double duration)
        {
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
        public static double CubicEaseIn(double time, double begin, double change, double duration)
        {
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
        public static double CubicEaseInOut(double time, double begin, double change, double duration)
        {
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
        public static double CubicEaseOutIn(double time, double begin, double change, double duration)
        {
            if (time < duration / 2)
                return CubicEaseOut(time * 2, begin, change / 2, duration);

            return CubicEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
        }

        #endregion

        #region Quartic

        /// <summary>
        /// Easing equation function for a quartic (time^4) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="begin">Starting value.</param>
        /// <param name="change">Final value.</param>
        /// <param name="duration">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuarticEaseOut(double time, double begin, double change, double duration)
        {
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
        public static double QuarticEaseIn(double time, double begin, double change, double duration)
        {
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
        public static double QuarticEaseInOut(double time, double begin, double change, double duration)
        {
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
        public static double QuarticEaseOutIn(double time, double begin, double change, double duration)
        {
            if (time < duration / 2)
                return QuarticEaseOut(time * 2, begin, change / 2, duration);

            return QuarticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
        }

        #endregion

        #region Quintic

        /// <summary>
        /// Easing equation function for a quintic (time^5) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="begin">Starting value.</param>
        /// <param name="change">Final value.</param>
        /// <param name="duration">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double QuinticEaseOut(double time, double begin, double change, double duration)
        {
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
        public static double QuinticEaseIn(double time, double begin, double change, double duration)
        {
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
        public static double QuinticEaseInOut(double time, double begin, double change, double duration)
        {
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
        public static double QuinticEaseOutIn(double time, double begin, double change, double duration)
        {
            if (time < duration / 2)
                return QuinticEaseOut(time * 2, begin, change / 2, duration);
            return QuinticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
        }

        #endregion

        #region Elastic

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="begin">Starting value.</param>
        /// <param name="change">Final value.</param>
        /// <param name="duration">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double ElasticEaseOut(double time, double begin, double change, double duration)
        {
            if ((time /= duration) == 1)
                return begin + change;

            double p = duration * .3;
            double s = p / 4;

            return (change * Math.Pow(2, -10 * time) * Math.Sin((time * duration - s) * (2 * Math.PI) / p) + change + begin);
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
        public static double ElasticEaseIn(double time, double begin, double change, double duration)
        {
            if ((time /= duration) == 1)
                return begin + change;

            double p = duration * .3;
            double s = p / 4;

            return -(change * Math.Pow(2, 10 * (time -= 1)) * Math.Sin((time * duration - s) * (2 * Math.PI) / p)) + begin;
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
        public static double ElasticEaseInOut(double time, double begin, double change, double duration)
        {
            if ((time /= duration / 2) == 2)
                return begin + change;

            double p = duration * (.3 * 1.5);
            double s = p / 4;

            if (time < 1)
                return -.5 * (change * Math.Pow(2, 10 * (time -= 1)) * Math.Sin((time * duration - s) * (2 * Math.PI) / p)) + begin;
            return change * Math.Pow(2, -10 * (time -= 1)) * Math.Sin((time * duration - s) * (2 * Math.PI) / p) * .5 + change + begin;
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
        public static double ElasticEaseOutIn(double time, double begin, double change, double duration)
        {
            if (time < duration / 2)
                return ElasticEaseOut(time * 2, begin, change / 2, duration);
            return ElasticEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
        }

        #endregion

        #region Bounce

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="begin">Starting value.</param>
        /// <param name="change">Final value.</param>
        /// <param name="duration">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BounceEaseOut(double time, double begin, double change, double duration)
        {
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
        public static double BounceEaseIn(double time, double begin, double change, double duration)
        {
            return change - BounceEaseOut(duration - time, 0, change, duration) + begin;
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
        public static double BounceEaseInOut(double time, double begin, double change, double duration)
        {
            if (time < duration / 2)
                return BounceEaseIn(time * 2, 0, change, duration) * .5 + begin;
            else
                return BounceEaseOut(time * 2 - duration, 0, change, duration) * .5 + change * .5 + begin;
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
        public static double BounceEaseOutIn(double time, double begin, double change, double duration)
        {
            if (time < duration / 2)
                return BounceEaseOut(time * 2, begin, change / 2, duration);
            return BounceEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
        }

        #endregion

        #region Back

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*time^3 - s*time^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="time">Current time in seconds.</param>
        /// <param name="begin">Starting value.</param>
        /// <param name="change">Final value.</param>
        /// <param name="duration">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseOut(double time, double begin, double change, double duration)
        {
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
        public static double BackEaseIn(double time, double begin, double change, double duration)
        {
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
        public static double BackEaseInOut(double time, double begin, double change, double duration)
        {
            double s = 1.70158;
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
        public static double BackEaseOutIn(double time, double begin, double change, double duration)
        {
            if (time < duration / 2)
                return BackEaseOut(time * 2, begin, change / 2, duration);
            return BackEaseIn((time * 2) - duration, begin + change / 2, change / 2, duration);
        }

        #endregion
    }
}