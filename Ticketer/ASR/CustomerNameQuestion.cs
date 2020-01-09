using Microsoft.Speech.Recognition;
using Ticketer.MyProperties;

namespace Ticketer.ASR
{
    class CustomerNameQuestion : ASRAbstract
    {
        private SpeechRecognitionEngine pSRE;

        public CustomerNameQuestion()
        {
            pSRE = new SpeechRecognitionEngine(MyCultureInfo.PolishCulture);
            pSRE.SetInputToDefaultAudioDevice();
            PrepareGrammar(pSRE);
        }

        public override string GetAnswer()
        {
            return AnswerReceiver.GetAnswer(pSRE);
        }

        protected override void PrepareGrammar(SpeechRecognitionEngine engine)
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            Choices names = GetNames();
            grammarBuilder.Append(names);
            Choices surnames = GetSurnames();
            grammarBuilder.Append(surnames);
            Grammar grammar = new Grammar(grammarBuilder);

            engine.LoadGrammar(grammar);
        }

        private Choices GetNames()
        {
            Choices names = new Choices();
            names.Add("Adam");
            names.Add("Kamil");
            names.Add("Piotr");
            names.Add("Krzysztof");
            names.Add("Karol");
            names.Add("Paweł");
            names.Add("Zbyszek");
            names.Add("Michał");
            names.Add("Jan");
            names.Add("Damian");
            names.Add("Rafał");
            names.Add("Dominik");
            names.Add("Gerard");
            names.Add("Janusz");
            names.Add("Bartłomiej");
            names.Add("Bartosz");
            names.Add("Bartek");
            names.Add("Karolina");
            names.Add("Kinga");
            names.Add("Jadwiga");
            names.Add("Halina");
            names.Add("Kasia");
            names.Add("Joanna");
            names.Add("Wioletta");
            names.Add("Elżbieta");
            names.Add("Janina");
            names.Add("Helena");
            names.Add("Adrianna");
            names.Add("Lucyna");
            names.Add("Henryk");
            names.Add("Tomasz");
            return names;
        }

        private Choices GetSurnames()
        {
            Choices surnames = new Choices();
            surnames.Add("Błaszczyk");
            surnames.Add("Kowalewicz");
            surnames.Add("Janowicz");
            surnames.Add("Borysewicz");
            surnames.Add("Henc");
            surnames.Add("Klepacz");
            surnames.Add("Grzyb");
            surnames.Add("Rutowicz");
            surnames.Add("Stonoga");
            surnames.Add("Duda");
            surnames.Add("Rokita");
            surnames.Add("Frankiewicz");
            surnames.Add("Banach");
            surnames.Add("Mazurek");
            surnames.Add("Kononowicz");
            surnames.Add("Nowak");
            surnames.Add("Owsiak");
            surnames.Add("Górnik");
            surnames.Add("Skowron");
            surnames.Add("Porażka");
            surnames.Add("Michalak");
            surnames.Add("Pałys");
            surnames.Add("Zaremba");
            surnames.Add("Lis");
            surnames.Add("Stępień");
            surnames.Add("Baranowski");
            return surnames;
        }
    }
}
