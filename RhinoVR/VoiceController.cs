using System;
using System.Speech.Recognition;
using System.Windows.Forms;

namespace RhinoVR {
  internal class VoiceController {
    private static bool _isEnabled = false;
    private static string _recognizedText = "";
    private static SpeechRecognitionEngine _recognizer;

    public static SpeechRecognitionEngine Recognizer {
      get {
        return VoiceController._recognizer;
      }
    }

    public static bool IsEnabled {
      get {
        return VoiceController._isEnabled;
      }
    }

    public static string RecognizedText {
      get {
        return VoiceController._recognizedText;
      }
      set {
        VoiceController._recognizedText = value;
      }
    }

    public static void InitVoiceController() {
      VoiceController._recognizer = new SpeechRecognitionEngine();
      Choices alternateChoices = new Choices();
      alternateChoices.Add("line", "rectangle", "circle", "move", "copy", "scale", "extrude", "cancel", "hide", "show", "ok", "delete", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "100", "1000", "10000", "100000");
      Grammar grammar = new Grammar(new GrammarBuilder(alternateChoices));
      VoiceController._recognizer.RequestRecognizerUpdate();
      VoiceController._recognizer.LoadGrammar(grammar);
      try {
        VoiceController._recognizer.SetInputToDefaultAudioDevice();
        VoiceController._recognizer.RecognizeAsync(RecognizeMode.Multiple);
      } catch {
        int num = (int)MessageBox.Show("No audio input device connected to this PC. Please connect a microphone and set as a default input in order to use Myo and Speech Recognition function", "Audio Input not found", MessageBoxButtons.OK, MessageBoxIcon.Question);
      }
      VoiceController._isEnabled = true;
    }

    public static void DinitVoiceController() {
      VoiceController._recognizer.Dispose();
      VoiceController._isEnabled = false;
    }

    public static void StartVoice() {
      if (!VoiceController.IsEnabled)
        return;
      VoiceController.Recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(VoiceController.Recognizer_SpeechRecognized);
    }

    public static void StopVoice() {
      if (!VoiceController.IsEnabled)
        return;
      VoiceController.Recognizer.SpeechRecognized -= new EventHandler<SpeechRecognizedEventArgs>(VoiceController.Recognizer_SpeechRecognized);
    }

    private static void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e) {
      VoiceController._recognizedText = e.Result.Text;
    }
  }
}
