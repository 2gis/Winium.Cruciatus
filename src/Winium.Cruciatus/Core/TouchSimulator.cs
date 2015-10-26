namespace Winium.Cruciatus.Core
{
    #region using

    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows;

    using Winium.Cruciatus.Elements;

    #endregion

    public static class TouchSimulator
    {
        #region Public Methods and Operators

        /// <summary>
        /// Double tap on the touch screen using finger motion events.
        /// </summary>
        /// <param name="element">The element to double tap on</param>
        /// <returns></returns>
        public static bool DoubleTap(CruciatusElement element)
        {
            return ElementCenterAction(element, DoubleTap);
        }

        /// <summary>
        /// Double tap on the touch screen using finger motion events.
        /// </summary>
        /// <param name="x">The X coordinate of the point to be tapped</param>
        /// <param name="y">The Y coordinate of the point to be tapped</param>
        /// <returns></returns>
        [DllImport(@"Winium.Cruciatus.TouchSimulator.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool DoubleTap(int x, int y);

        /// <summary>
        /// Flick on the touch screen using finger motion events. The flick begins in the center of the screen.
        /// </summary>
        /// <param name="xSpeed">The X speed in pixels per second</param>
        /// <param name="ySpeed">The Y speed in pixels per second</param>
        /// <returns></returns>
        public static bool Flick(int xSpeed, int ySpeed)
        {
            var gestureMs = 250;

            var gestureSeconds = gestureMs / 1000.0;

            var element = CruciatusFactory.Root;

            var rect = element.Properties.BoundingRectangle;

            var startPoint = new Point(
                rect.Left + (rect.Width / 2),
                rect.Top + (rect.Height / 2));

            var xPixels = xSpeed * gestureSeconds;
            var yPixels = ySpeed * gestureSeconds;

            var endPoint = new Point(startPoint.X + xPixels, startPoint.Y + yPixels);

            if (!TouchDown((int)startPoint.X, (int)startPoint.Y))
            {
                return false;
            }

            var startTime = DateTime.Now;

            while (DateTime.Now < startTime + TimeSpan.FromMilliseconds(gestureMs))
            {
                var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                var elapsedFraction = elapsed / gestureMs;

                var xAdjustment = xPixels == 0 ? 0 : xPixels * elapsedFraction;
                var yAdjustment = yPixels == 0 ? 0 : yPixels * elapsedFraction;

                if (!TouchUpdate((int)(startPoint.X + xAdjustment), (int)(startPoint.Y + yAdjustment)))
                {
                    return false;
                }

                Thread.Sleep(16);
            }

            return TouchUpdate((int)endPoint.X, (int)endPoint.Y)
                && TouchUp((int)endPoint.X, (int)endPoint.Y);
        }

        /// <summary>
        /// Flick on the touch screen using finger motion events.
        /// </summary>
        /// <param name="element">The element where the flick starts</param>
        /// <param name="xOffset">The X offset in pixels to flick by</param>
        /// <param name="yOffset">The Y offset in pixels to flixk by</param>
        /// <param name="pixelsPerSecond">The speed in pixels per second</param>
        /// <returns></returns>
        public static bool FlickElement(CruciatusElement element, int xOffset, int yOffset, int pixelsPerSecond)
        {
            var rect = element.Properties.BoundingRectangle;

            var startPoint = new Point(
                rect.Left + (rect.Width / 2),
                rect.Top + (rect.Height / 2));

            var endPoint = new Point(
                startPoint.X + xOffset,
                startPoint.Y + yOffset);

            if (!TouchDown((int)startPoint.X, (int)startPoint.Y))
            {
                return false;
            }

            var distance = Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2));

            var gestureMilliseconds = distance / pixelsPerSecond * 1000;

            var startTime = DateTime.Now;

            while (DateTime.Now < (startTime + TimeSpan.FromMilliseconds(gestureMilliseconds)))
            {
                var elapsed = (DateTime.Now - startTime).TotalMilliseconds;

                var elapsedFraction = elapsed / gestureMilliseconds;

                if (!TouchUpdate(
                        (int)(startPoint.X + (xOffset * elapsedFraction)),
                        (int)(startPoint.Y + (yOffset * elapsedFraction))))
                {
                    return false;
                }

                Thread.Sleep(16);
            }

            return TouchUpdate((int)endPoint.X, (int)endPoint.Y)
                && TouchUp((int)endPoint.X, (int)endPoint.Y);

        }

        /// <summary>
        /// Single tap on the touch enabled device.
        /// </summary>
        /// <param name="element">The element to single tap on</param>
        /// <returns></returns>
        public static bool Tap(CruciatusElement element)
        {
            return ElementCenterAction(element, Tap);
        }

        /// <summary>
        /// Single tap on the touch enabled device.
        /// </summary>
        /// <param name="x">The X coordinate of the point to tap</param>
        /// <param name="y">The Y coordinate of the point to tap</param>
        /// <returns></returns>
        [DllImport(@"Winium.Cruciatus.TouchSimulator.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Tap(int x, int y);

        /// <summary>
        /// Finger down on the screen.
        /// </summary>
        /// <param name="element">The element to touch</param>
        /// <param name="xOffset">The X coordinate relative to the element</param>
        /// <param name="yOffset">The Y coordinate relative to the element</param>
        /// <returns></returns>
        public static bool TouchDown(CruciatusElement element, int xOffset, int yOffset)
        {
            return ElementLocationAction(element, xOffset, yOffset, TouchDown);
        }

        /// <summary>
        /// Finger down on the screen
        /// </summary>
        /// <param name="x">The X coordinate on the screen</param>
        /// <param name="y">The Y coordinate on the screen</param>
        /// <returns></returns>
        [DllImport(@"Winium.Cruciatus.TouchSimulator.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool TouchDown(int x, int y);

        /// <summary>
        /// Finger up on the screen.
        /// </summary>
        /// <param name="element">The element to touch</param>
        /// <param name="xOffset">The X coordinate relative to the element</param>
        /// <param name="yOffset">The Y coordinate relative to the element</param>
        /// <returns></returns>
        public static bool TouchUp(CruciatusElement element, int xOffset, int yOffset)
        {
            return ElementLocationAction(element, xOffset, yOffset, TouchUp);
        }

        /// <summary>
        /// Finger up on the screen
        /// </summary>
        /// <param name="x">The X coordinate on the screen</param>
        /// <param name="y">The Y coordinate on the screen</param>
        /// <returns></returns>
        [DllImport(@"Winium.Cruciatus.TouchSimulator.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool TouchUp(int x, int y);

        /// <summary>
        /// Finger move on the screen.
        /// </summary>
        /// <param name="element">The element to touch</param>
        /// <param name="xOffset">The X coordinate relative to the element</param>
        /// <param name="yOffset">The Y coordinate relative to the element</param>
        /// <returns></returns>
        public static bool TouchUpdate(CruciatusElement element, int xOffset, int yOffset)
        {
            return ElementLocationAction(element, xOffset, yOffset, TouchUpdate);
        }

        /// <summary>
        /// Finger move on the screen
        /// </summary>
        /// <param name="x">The X coordinate on the screen</param>
        /// <param name="y">The Y coordinate on the screen</param>
        /// <returns></returns>
        [DllImport(@"Winium.Cruciatus.TouchSimulator.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool TouchUpdate(int x, int y);

        /// <summary>
        /// Long press on the touch screen using finger motion events.
        /// </summary>
        /// <param name="element">The element to long press on</param>
        /// <param name="duration">The duration of the press</param>
        /// <returns></returns>
        public static bool LongTap(CruciatusElement element, int duration)
        {
            int x, y;
            ElementCenter(element, out x, out y);

            return LongTap(x, y, duration);
        }

        /// <summary>
        /// Long press on the touch screen using finger motion events.
        /// </summary>
        /// <param name="element">The element to long press on</param>
        /// <param name="xOffset">The X offset of the element being touched</param>
        /// <param name="yOffset">The Y offset of the element being touched</param>
        /// <param name="duration">The duration of the press</param>
        /// <returns></returns>
        public static bool LongTap(CruciatusElement element, int xOffset, int yOffset, int duration)
        {
            var rect = element.Properties.BoundingRectangle;
            
            return LongTap((int)(rect.Left + xOffset), (int)(rect.Top + yOffset), duration);
        }

        /// <summary>
        /// Long press on the touch screen using finger motion events.
        /// </summary>
        /// <param name="x">The X coordinate on the screen</param>
        /// <param name="y">The Y coordinate on the screen</param>
        /// <param name="duration">The duration of the press</param>
        /// <returns></returns>
        [DllImport(@"Winium.Cruciatus.TouchSimulator.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LongTap(int x, int y, int duration);

        /// <summary>
        /// Scroll on the touch screen using finger based motion events.
        /// </summary>
        /// <param name="element">The element to scroll</param>
        /// <param name="xOffset">The X pixels to scroll</param>
        /// <param name="yOffset">The Y pixels to scroll</param>
        /// <returns></returns>
        public static bool Scroll(CruciatusElement element, int xOffset, int yOffset)
        {
            var rect = element.Properties.BoundingRectangle;

            var startPoint = new Point(
                rect.Left + (rect.Width / 2),
                rect.Top + (rect.Height / 2));

            var endPoint = new Point(
                startPoint.X + xOffset,
                startPoint.Y + yOffset);

            if (!TouchDown((int)startPoint.X, (int)startPoint.Y))
            {
                return false;
            }

            var distance = Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2));

            for (var soFar = 6; soFar < distance; soFar += 6)
            {
                var soFarFraction = soFar / distance;

                var x = (int)(startPoint.X + (xOffset * soFarFraction));
                var y = (int)(startPoint.Y + (yOffset * soFarFraction));
                if (!TouchUpdate(x, y))
                {
                    return false;
                }

                Thread.Sleep(8);
            }

            var startTime = DateTime.Now;
            while (DateTime.Now < (startTime + TimeSpan.FromMilliseconds(500)))
            {
                if (!TouchUpdate((int)endPoint.X, (int)endPoint.Y))
                {
                    return false;
                }
                Thread.Sleep(16);
            }

            return TouchUp((int)endPoint.X, (int)endPoint.Y);
        }

        #endregion

        #region Methods

        private static bool ElementCenterAction(CruciatusElement element, Func<int, int, bool> action)
        {
            int x, y;
            ElementCenter(element, out x, out y);

            return action(x, y);
        }

        private static void ElementCenter(CruciatusElement element, out int x, out int y)
        {
            var rect = element.Properties.BoundingRectangle;

            x = (int)(rect.Left + (rect.Width / 2));
            y = (int)(rect.Left + (rect.Height / 2));
        }

        private static bool ElementLocationAction(
            CruciatusElement element,
            int xOffset,
            int yOffset,
            Func<int, int, bool> action)
        {
            var rect = element.Properties.BoundingRectangle;

            return action((int)(rect.Left + xOffset), (int)(rect.Top + yOffset));
        }

        #endregion
    }
}
