using System;
using System.Globalization;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VertaIT.WPF.Theme.Controls
{
    [TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = IncreaseButtonPartName, Type = typeof(ButtonBase))]
    [TemplatePart(Name = DecreaseButtonPartName, Type = typeof(ButtonBase))]
    public class NumericUpDown : RangeBase
    {
        private const string TextBoxPartName = "PART_TextBox";
        private const string IncreaseButtonPartName = "PART_IncreaseButton";
        private const string DecreaseButtonPartName = "PART_DecreaseButton";

        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(
            nameof(DecimalPlaces),
            typeof(int),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(0, OnDecimalPlacesChanged),
            value => (int)value >= 0 && (int)value <= 15);

        public static readonly DependencyProperty IsReadOnlyProperty = TextBoxBase.IsReadOnlyProperty.AddOwner(
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(false, OnIsReadOnlyChanged));

        private TextBox _textBox;
        private ButtonBase _increaseButton;
        private ButtonBase _decreaseButton;

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
            MaximumProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(100.0));
            SmallChangeProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(1.0));
            LargeChangeProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(10.0));
        }

        /// <summary>
        /// Number of digits shown after the decimal separator. Values are rounded to this precision.
        /// </summary>
        public int DecimalPlaces
        {
            get => (int)GetValue(DecimalPlacesProperty);
            set => SetValue(DecimalPlacesProperty, value);
        }

        /// <summary>
        /// When true, the value can't be changed by the user (text input, buttons, keyboard or mouse wheel).
        /// </summary>
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public override void OnApplyTemplate()
        {
            if (_textBox != null)
            {
                _textBox.LostKeyboardFocus -= TextBox_LostKeyboardFocus;
            }
            if (_increaseButton != null)
            {
                _increaseButton.Click -= IncreaseButton_Click;
            }
            if (_decreaseButton != null)
            {
                _decreaseButton.Click -= DecreaseButton_Click;
            }

            base.OnApplyTemplate();

            _textBox = GetTemplateChild(TextBoxPartName) as TextBox;
            _increaseButton = GetTemplateChild(IncreaseButtonPartName) as ButtonBase;
            _decreaseButton = GetTemplateChild(DecreaseButtonPartName) as ButtonBase;

            if (_textBox != null)
            {
                _textBox.LostKeyboardFocus += TextBox_LostKeyboardFocus;
            }
            if (_increaseButton != null)
            {
                _increaseButton.Click += IncreaseButton_Click;
            }
            if (_decreaseButton != null)
            {
                _decreaseButton.Click += DecreaseButton_Click;
            }

            UpdateText();
            UpdateButtons();
        }

        /// <summary>
        /// Increases the value by <see cref="RangeBase.SmallChange"/>.
        /// </summary>
        public void Increase() => ChangeValueBy(SmallChange);

        /// <summary>
        /// Decreases the value by <see cref="RangeBase.SmallChange"/>.
        /// </summary>
        public void Decrease() => ChangeValueBy(-SmallChange);

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            UpdateText();
            UpdateButtons();
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            UpdateButtons();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            UpdateButtons();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (e.NewFocus == this && _textBox != null)
            {
                _textBox.Focus();
                _textBox.SelectAll();
                e.Handled = true;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            switch (e.Key)
            {
                case Key.Up:
                    CommitText();
                    ChangeValueBy(SmallChange);
                    e.Handled = true;
                    break;
                case Key.Down:
                    CommitText();
                    ChangeValueBy(-SmallChange);
                    e.Handled = true;
                    break;
                case Key.PageUp:
                    CommitText();
                    ChangeValueBy(LargeChange);
                    e.Handled = true;
                    break;
                case Key.PageDown:
                    CommitText();
                    ChangeValueBy(-LargeChange);
                    e.Handled = true;
                    break;
                case Key.Enter:
                    CommitText();
                    _textBox?.SelectAll();
                    e.Handled = true;
                    break;
                case Key.Escape:
                    UpdateText();
                    _textBox?.SelectAll();
                    e.Handled = true;
                    break;
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (!e.Handled && IsKeyboardFocusWithin)
            {
                CommitText();
                ChangeValueBy(e.Delta > 0 ? SmallChange : -SmallChange);
                e.Handled = true;
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer() => new NumericUpDownAutomationPeer(this);

        private static void OnDecimalPlacesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericUpDown = (NumericUpDown)d;
            numericUpDown.Value = numericUpDown.Round(numericUpDown.Value);
            numericUpDown.UpdateText();
        }

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((NumericUpDown)d).UpdateButtons();
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e) => ChangeValueBy(SmallChange);

        private void DecreaseButton_Click(object sender, RoutedEventArgs e) => ChangeValueBy(-SmallChange);

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => CommitText();

        private void ChangeValueBy(double delta)
        {
            if (IsReadOnly)
            {
                return;
            }

            Value = Round(Value + delta);
        }

        private void CommitText()
        {
            if (_textBox == null || IsReadOnly)
            {
                return;
            }

            if (double.TryParse(_textBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out double parsedValue))
            {
                Value = Round(parsedValue);
            }

            // Shows the actual value when the text was invalid or the value was coerced to the range.
            UpdateText();
        }

        private double Round(double value) => Math.Round(value, DecimalPlaces, MidpointRounding.AwayFromZero);

        private void UpdateText()
        {
            if (_textBox != null)
            {
                _textBox.Text = Value.ToString("F" + DecimalPlaces, CultureInfo.CurrentCulture);
            }
        }

        private void UpdateButtons()
        {
            if (_increaseButton != null)
            {
                _increaseButton.IsEnabled = !IsReadOnly && Value < Maximum;
            }
            if (_decreaseButton != null)
            {
                _decreaseButton.IsEnabled = !IsReadOnly && Value > Minimum;
            }
        }
    }

    public class NumericUpDownAutomationPeer : RangeBaseAutomationPeer
    {
        public NumericUpDownAutomationPeer(NumericUpDown owner)
            : base(owner)
        {
        }

        protected override string GetClassNameCore() => nameof(NumericUpDown);

        protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.Spinner;
    }
}