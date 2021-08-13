using System.Drawing;
using System.Linq;
using Ghpp;
using static System.Console;

var command = "ghpp";
var output = new Output();

foreach (var arg in args)
{
    switch (command)
    {
        default:
            var lowarg = arg.ToLower();

            switch (lowarg)
            {
                default:
                    command = lowarg;
                    break;

                case "-h":
                    output.Path = null;
                    break;

                case "-sm":
                    output.Map = Map.GetSymmetricMap();
                    break;

                case "-db":
                    output.BackgroundColor = Color.FromArgb(240, 240, 240);
                    break;
            }
            break;

        case "ghpp":
            if (arg.ToLower() != "-h")
                output.Path = arg;

            command = string.Empty;
            break;

        case "-m":
            output.Map = uint.TryParse(arg, out var bits) ? new Map(bits) : new Map();
            command = string.Empty;
            break;

        case "-b" or "-f":
            var strings = arg.Split(',');
            byte b = 0;

            var rgb = from s in strings
                      where byte.TryParse(s, out b)
                      select b;

            while (rgb.Count() < 3)
                rgb = rgb.Append<byte>(0);

            var color = Color.FromArgb(rgb.ElementAt(0), rgb.ElementAt(1), rgb.ElementAt(2));

            if (command == "-b")
                output.BackgroundColor = color;
            else
                output.ForegroundColor = color;

            command = string.Empty;
            break;
    }
}

if (!output.Save())
{
    Write(
        "\nUsage: ghpp <OUTPUT> [-sm] [-m <MAP>] [-db] [-b <R>,<G>,<B>] [-f <R>,<G>,<B>]\n\n" +
        "Variables:\n" +
        "    OUTPUT    The output image path.\n" +
        "    MAP       An integer from 0 to 33554431 that defines the image pattern.\n" +
        "    R         An integer from 0 to 255 that defines the red channel of the color.\n" +
        "    G         An integer from 0 to 255 that defines the green channel of the color.\n" +
        "    B         An integer from 0 to 255 that defines the blue channel of the color.\n\n" +
        "Arguments:\n" +
        "    -sm       Uses a symmetric map.\n" +
        "    -m        Uses <MAP> as the image pattern.\n" +
        "    -db       Uses the default GitHub profile picture beckground color.\n" +
        "    -b        Uses <R>, <G> and <B> as the background color.\n" +
        "    -f        Uses <R>, <G> and <B> as the foreground color.\n\n" +
        "Examples:\n" +
        "    ghpp image.png\n" +
        "    ghpp image.png -b 255,255,255\n" +
        "    ghpp image.png -f 0,0,0\n" +
        "    ghpp image.png -b 255,255,255 -f 0,0,0\n" +
        "    ghpp image.png -m 4681156\n" +
        "    ghpp image.png -m 4681156 -b 255,255,255\n" +
        "    ghpp image.png -m 4681156 -f 0,0,0\n" +
        "    ghpp image.png -m 4681156 -b 255,255,255 -f 0,0,0\n\n"
    );
}
