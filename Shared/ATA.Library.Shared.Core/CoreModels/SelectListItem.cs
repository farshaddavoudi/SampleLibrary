﻿namespace ATA.Library.Shared.Core.CoreModels
{
    /// <summary>
    /// This class is typically rendered as an HTML <code>&lt;option&gt;</code> element with the specified
    /// attribute values.
    /// </summary>
    public class SelectListItem
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SelectListItem"/>.
        /// </summary>
        public SelectListItem() { }

        /// <summary>
        /// Initializes a new instance of <see cref="SelectListItem"/>.
        /// </summary>
        /// <param name="text">The display text of this <see cref="SelectListItem"/>.</param>
        /// <param name="value">The value of this <see cref="SelectListItem"/>.</param>
        public SelectListItem(string? text, string value)
            : this()
        {
            Text = text;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SelectListItem"/>.
        /// </summary>
        /// <param name="text">The display text of this <see cref="SelectListItem"/>.</param>
        /// <param name="value">The value of this <see cref="SelectListItem"/>.</param>
        /// <param name="selected">Value that indicates whether this <see cref="SelectListItem"/> is selected.</param>
        public SelectListItem(string text, string value, bool selected)
            : this(text, value)
        {
            Selected = selected;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SelectListItem"/>.
        /// </summary>
        /// <param name="text">The display text of this <see cref="SelectListItem"/>.</param>
        /// <param name="value">The value of this <see cref="SelectListItem"/>.</param>
        /// <param name="selected">Value that indicates whether this <see cref="SelectListItem"/> is selected.</param>
        /// <param name="disabled">Value that indicates whether this <see cref="SelectListItem"/> is disabled.</param>
        public SelectListItem(string text, string value, bool selected, bool disabled)
            : this(text, value, selected)
        {
            Disabled = disabled;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether this <see cref="SelectListItem"/> is disabled.
        /// This property is typically rendered as a <code>disabled="disabled"</code> attribute in the HTML
        /// <code>&lt;option&gt;</code> element.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether this <see cref="SelectListItem"/> is selected.
        /// This property is typically rendered as a <code>selected="selected"</code> attribute in the HTML
        /// <code>&lt;option&gt;</code> element.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates the display text of this <see cref="SelectListItem"/>.
        /// This property is typically rendered as the inner HTML in the HTML <code>&lt;option&gt;</code> element.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates the value of this <see cref="SelectListItem"/>.
        /// This property is typically rendered as a <code>value="..."</code> attribute in the HTML
        /// <code>&lt;option&gt;</code> element.
        /// </summary>
        public string? Value { get; set; }
    }
}