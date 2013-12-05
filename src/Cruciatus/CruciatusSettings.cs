// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CruciatusSettings.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет класс настроек Cruciatus.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus
{
    using Microsoft.VisualStudio.TestTools.UITesting;

    public class CruciatusSettings
    {
        private const int DefaultMouseMoveSpeed = 2500;

        private int mouseMoveSpeed;

        internal CruciatusSettings()
        {
            this.ResetToDefault();
        }

        public int MouseMoveSpeed
        {
            get
            {
                return this.mouseMoveSpeed;
            }

            set
            {
                this.mouseMoveSpeed = value;
                Mouse.MouseMoveSpeed = value;
            }
        }

        /// <summary>
        /// Сбрасывает значения настроек на исходные.
        /// </summary>
        public void ResetToDefault()
        {
            this.MouseMoveSpeed = DefaultMouseMoveSpeed;
        }
    }
}