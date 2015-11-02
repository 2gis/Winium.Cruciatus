namespace Winium.Cruciatus.Core
{
    #region using

    using System;
    using System.Threading;
    using System.Windows;

    using TCD.System.TouchInjection;

    using Winium.Cruciatus.Elements;

    #endregion

    public static class TouchSimulator
    {
        #region Fields

        private static bool _touchInjectionInitialized;

        private static int _pauseBeforeUp = 300; // after drag, delay 'up' this long to avoid inertia

        private static PointerTouchInfo _contact;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Double tap on the touch screen using finger motion events.
        /// </summary>
        /// <param name="element">The element to double tap on</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool DoubleTap(CruciatusElement element)
        {
            return ElementCenterAction(element, DoubleTap);
        }

        /// <summary>
        /// Double tap on the touch screen using finger motion events.
        /// </summary>
        /// <param name="x">The X coordinate of the point to be tapped</param>
        /// <param name="y">The Y coordinate of the point to be tapped</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool DoubleTap(int x, int y)
        {
            if (!Tap(x, y))
            {
                return false;
            }

            Thread.Sleep(200);

            return Tap(x, y);
        }

        public static bool Flick(int xStart, int yStart, int xEnd, int yEnd, int gestureTime)
        {
            if (!TouchDown(xStart, yStart))
            {
                return false;
            }

            var startTime = DateTime.UtcNow;

            var iterations = 0;

            while (DateTime.UtcNow < startTime + TimeSpan.FromMilliseconds(gestureTime))
            {
                iterations++;

                var elapsed = (DateTime.UtcNow - startTime).TotalMilliseconds;
                var elapsedFraction = elapsed / gestureTime;

                var xAdjustment = (xEnd - xStart) * elapsedFraction;
                var yAdjustment = (yEnd - yStart) * elapsedFraction;

                if (!TouchUpdate((int)(xStart + xAdjustment), (int)(yStart + yAdjustment)))
                {
                    return false;
                }

                while ((DateTime.UtcNow - startTime).TotalMilliseconds < (iterations * 16))
                {
                    Thread.Sleep(4);
                }
            }

            return TouchUpdate(xEnd, yEnd) && TouchUp(xEnd, yEnd);
        }

        /// <summary>
        /// Flick on the touch screen using finger motion events. The flick begins in the center of the screen.
        /// </summary>
        /// <param name="xSpeed">The X speed in pixels per second</param>
        /// <param name="ySpeed">The Y speed in pixels per second</param>
        /// <returns>true if successful, otherwise false</returns>
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

            var startTime = DateTime.UtcNow;

            var iterations = 0;

            while (DateTime.UtcNow < startTime + TimeSpan.FromMilliseconds(gestureMs))
            {
                iterations++;

                var elapsed = (DateTime.UtcNow - startTime).TotalMilliseconds;
                var elapsedFraction = elapsed / gestureMs;

                var xAdjustment = xPixels == 0 ? 0 : xPixels * elapsedFraction;
                var yAdjustment = yPixels == 0 ? 0 : yPixels * elapsedFraction;

                if (!TouchUpdate((int)(startPoint.X + xAdjustment), (int)(startPoint.Y + yAdjustment)))
                {
                    return false;
                }

                while ((DateTime.UtcNow - startTime).TotalMilliseconds < (iterations * 16))
                {
                    Thread.Sleep(4);
                }
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
        /// <returns>true if successful, otherwise false</returns>
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

            var startTime = DateTime.UtcNow;

            var iterations = 0;

            while (DateTime.UtcNow < (startTime + TimeSpan.FromMilliseconds(gestureMilliseconds)))
            {
                var elapsed = (DateTime.UtcNow - startTime).TotalMilliseconds;

                var elapsedFraction = elapsed / gestureMilliseconds;

                if (!TouchUpdate(
                        (int)(startPoint.X + (xOffset * elapsedFraction)),
                        (int)(startPoint.Y + (yOffset * elapsedFraction))))
                {
                    return false;
                }

                while ((DateTime.UtcNow - startTime).TotalMilliseconds < (iterations * 16))
                {
                    Thread.Sleep(4);
                }
            }

            return TouchUpdate((int)endPoint.X, (int)endPoint.Y)
                && TouchUp((int)endPoint.X, (int)endPoint.Y);

        }

        /// <summary>
        /// Single tap on the touch enabled device.
        /// </summary>
        /// <param name="element">The element to single tap on</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool Tap(CruciatusElement element)
        {
            return ElementCenterAction(element, Tap);
        }

        /// <summary>
        /// Single tap on the touch enabled device.
        /// </summary>
        /// <param name="x">The X coordinate of the point to tap</param>
        /// <param name="y">The Y coordinate of the point to tap</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool Tap(int x, int y)
        {
            if (!TouchDown(x, y))
            {
                return false;
            }

            return TouchUp(x, y);
        }

        /// <summary>
        /// Finger down on the screen.
        /// </summary>
        /// <param name="element">The element to touch</param>
        /// <param name="xOffset">The X coordinate relative to the element</param>
        /// <param name="yOffset">The Y coordinate relative to the element</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool TouchDown(CruciatusElement element, int xOffset, int yOffset)
        {
            return ElementLocationAction(element, xOffset, yOffset, TouchDown);
        }

        /// <summary>
        /// Finger down on the screen
        /// </summary>
        /// <param name="x">The X coordinate on the screen</param>
        /// <param name="y">The Y coordinate on the screen</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool TouchDown(int x, int y)
        {
            InitializeTouchInjection();

            _contact = new PointerTouchInfo();
            _contact.PointerInfo.pointerType = PointerInputType.TOUCH;
            _contact.PointerInfo.PointerId = 0;
            _contact.PointerInfo.PtPixelLocation.X = x;
            _contact.PointerInfo.PtPixelLocation.Y = y;

            _contact.TouchFlags = TouchFlags.NONE;
            _contact.TouchMasks = TouchMask.CONTACTAREA | TouchMask.ORIENTATION | TouchMask.PRESSURE;
            _contact.Orientation = 90;
            _contact.Pressure = 32000;

            _contact.ContactArea.top = _contact.PointerInfo.PtPixelLocation.Y - 2;
            _contact.ContactArea.bottom = _contact.PointerInfo.PtPixelLocation.Y + 2;
            _contact.ContactArea.left = _contact.PointerInfo.PtPixelLocation.X - 2;
            _contact.ContactArea.right = _contact.PointerInfo.PtPixelLocation.X + 2;

            _contact.PointerInfo.PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;

            return TouchInjector.InjectTouchInput(1, new [] { _contact });
        }

        /// <summary>
        /// Finger up on the screen.
        /// </summary>
        /// <param name="element">The element to touch</param>
        /// <param name="xOffset">The X coordinate relative to the element</param>
        /// <param name="yOffset">The Y coordinate relative to the element</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool TouchUp(CruciatusElement element, int xOffset, int yOffset)
        {
            return ElementLocationAction(element, xOffset, yOffset, TouchUp);
        }

        /// <summary>
        /// Finger up on the screen
        /// </summary>
        /// <param name="x">The X coordinate on the screen</param>
        /// <param name="y">The Y coordinate on the screen</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool TouchUp(int x, int y)
        {
            _contact.PointerInfo.PtPixelLocation.X = x;
            _contact.PointerInfo.PtPixelLocation.Y = y;

            _contact.PointerInfo.PointerFlags = PointerFlags.UP;
            return TouchInjector.InjectTouchInput(1, new[] { _contact });
        }

        /// <summary>
        /// Finger move on the screen.
        /// </summary>
        /// <param name="element">The element to touch</param>
        /// <param name="xOffset">The X coordinate relative to the element</param>
        /// <param name="yOffset">The Y coordinate relative to the element</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool TouchUpdate(CruciatusElement element, int xOffset, int yOffset)
        {
            return ElementLocationAction(element, xOffset, yOffset, TouchUpdate);
        }

        /// <summary>
        /// Finger move on the screen
        /// </summary>
        /// <param name="x">The X coordinate on the screen</param>
        /// <param name="y">The Y coordinate on the screen</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool TouchUpdate(int x, int y)
        {
            _contact.PointerInfo.PtPixelLocation.X = x;
            _contact.PointerInfo.PtPixelLocation.Y = y;

            _contact.PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;

            return TouchInjector.InjectTouchInput(1, new[] { _contact });
        }

        /// <summary>
        /// Long press on the touch screen using finger motion events.
        /// </summary>
        /// <param name="element">The element to long press on</param>
        /// <param name="duration">The duration of the press</param>
        /// <returns>true if successful, otherwise false</returns>
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
        /// <returns>true if successful, otherwise false</returns>
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
        /// <returns>true if successful, otherwise false</returns>
        public static bool LongTap(int x, int y, int duration)
        {
            if (!TouchDown(x, y))
            {
                return false;
            }

            var start = DateTime.UtcNow;

            while (DateTime.UtcNow < start + TimeSpan.FromMilliseconds(duration))
            {
                if (!TouchUpdate(x, y))
                {
                    return false;
                }
                Thread.Sleep(16);
            }

            return TouchUp(x, y);
        }

        /// <summary>
        /// Scroll on the touch screen using finger based motion events.
        /// </summary>
        /// <param name="xStart">The X coordinate to start the scroll</param>
        /// <param name="yStart">The Y coordinate to start the scroll</param>
        /// <param name="xEnd">The X coordinate to end the scroll</param>
        /// <param name="yEnd">The Y coordinate to end the scroll</param>
        /// <param name="dragTime">The time taken to perform the drag.  Defaults to the time required for 6 pixels per 8 ms.</param>
        /// <param name="pauseBeforeUp">The time to wait after scrolling before the 'up' gesture.  300 ms is sufficient to avoid inertia.</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool Scroll(int xStart, int yStart, int xEnd, int yEnd, int? dragTime = null, int pauseBeforeUp = 300)
        {

            if (!TouchDown(xStart, yStart))
            {
                return false;
            }

            if (!MoveTo(xStart, yStart, xEnd, yEnd, dragTime))
            {
                return false;
            }

            var startTime = DateTime.UtcNow;

            if (pauseBeforeUp > 0)
            {
                while (DateTime.UtcNow < (startTime + TimeSpan.FromMilliseconds(pauseBeforeUp)))
                {
                    if (!TouchUpdate(xEnd, yEnd))
                    {
                        return false;
                    }
                    Thread.Sleep(16);
                }
            }

            return TouchUp(xEnd, yEnd);
        }

        /// <summary>
        /// Drags a previously touched finger using finger based motion events.
        /// </summary>
        /// <param name="xStart">The X coordinate to start the gesture</param>
        /// <param name="yStart">The Y coordinate to start the gesture</param>
        /// <param name="xEnd">The X coordinate to end the gesture</param>
        /// <param name="yEnd">The Y coordinate to end the gesture</param>
        /// <param name="dragTime">The time taken to perform the drag.  Defaults to the time required for 6 pixels per 8 ms.</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool MoveTo(int xStart, int yStart, int xEnd, int yEnd, int? dragTime = null)
        {
            var xDistance = xEnd - xStart;
            var yDistance = yEnd - yStart;

            var distance = Math.Sqrt(Math.Pow(xStart - xEnd, 2) + Math.Pow(yStart - yEnd, 2));

            var stepTime = 16;

            if (!dragTime.HasValue)
            {
                dragTime = (int)((distance / 6) * stepTime);  // default to 6 pixels per step
            }
            var steps = dragTime / stepTime;

            var distancePerStep = distance / steps;

            var startTime = DateTime.UtcNow;

            for (var soFar = distancePerStep; soFar < distance; soFar += distancePerStep)
            {
                var soFarFraction = soFar / distance;

                var x = (int)(xStart + (xDistance * soFarFraction));
                var y = (int)(yStart + (yDistance * soFarFraction));

                if (!TouchUpdate(x, y))
                {
                    return false;
                }

                while ((DateTime.UtcNow - startTime).TotalMilliseconds < dragTime * soFarFraction)
                {
                    Thread.Sleep(4);
                }
            }

            return TouchUpdate(xEnd, yEnd);
        }

        /// <summary>
        /// Scroll on the touch screen using finger based motion events.
        /// </summary>
        /// <param name="element">The element to scroll</param>
        /// <param name="xOffset">The X pixels to scroll</param>
        /// <param name="yOffset">The Y pixels to scroll</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool Scroll(CruciatusElement element, int xOffset, int yOffset)
        {
            var rect = element.Properties.BoundingRectangle;

            var xStart = (int)(rect.Left + (rect.Width / 2));
            var yStart = (int)(rect.Top + (rect.Height / 2));

            var xEnd = xStart + xOffset;
            var yEnd = yStart + yOffset;

            return Scroll(xStart, yStart, xEnd, yEnd);
        }

        #endregion

        #region Methods

        private static bool InitializeTouchInjection()
        {
            if (!_touchInjectionInitialized)
            {
                _touchInjectionInitialized = TouchInjector.InitializeTouchInjection();
            }

            return _touchInjectionInitialized;
        }

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
            y = (int)(rect.Top + (rect.Height / 2));
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
