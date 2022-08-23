// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

namespace LPMobile.Views;

public class HyperlinkSpan : Span
{
    public static readonly BindableProperty UrlProperty =
        BindableProperty.Create(nameof(Url), typeof(string), typeof(HyperlinkSpan), null);

    public string Url
    {
        get => (string)this.GetValue(UrlProperty);
        set => this.SetValue(UrlProperty, value);
    }

    public HyperlinkSpan()
    {
        this.TextDecorations = TextDecorations.Underline;
        this.TextColor = Colors.Blue;

        var command = new Command(async () => await Launcher.OpenAsync(this.Url));
        var recognizer = new TapGestureRecognizer();
        recognizer.Command = command;
        // recognizer.Tapped += (sender, args) => { command.Execute(null); };
        this.GestureRecognizers.Add(recognizer);
    }

    private void Recognizer_Tapped(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}
