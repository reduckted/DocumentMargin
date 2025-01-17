﻿using System.Linq;
using System.Windows;
using DocumentMargin.Margins;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text.Editor;

namespace DocumentMargin.Margin
{
    internal class SelectionMargin : BaseMargin
    {
        public override string MarginName => "Selection Margin";
        private readonly ITextView2 _view;
        private bool _isDisposed;

        public SelectionMargin(ITextView view)
        {
            _view = (ITextView2)view;
            _view.Selection.SelectionChanged += OnSelectionChanged;

            SetResourceReference(BackgroundProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);
            SetResourceReference(ForegroundProperty, EnvironmentColors.ComboBoxFocusedTextBrushKey);
            FontSize = 11;
            Margin = new Thickness(0, 0, 0, 0);
            Padding = new Thickness(9, 1, 9, 0);
            Visibility = Visibility.Collapsed;

            SetSelection();
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            SetSelection();
        }

        private void SetSelection()
        {
            var length = _view.MultiSelectionBroker.AllSelections.Select(s => s.Extent.Length).Sum();

            if (length > 0)
            {
                Text = $"Sel: {length}";
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }

        public override void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;

                _view.Selection.SelectionChanged -= OnSelectionChanged;
            }
        }
    }
}
