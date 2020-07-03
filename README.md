# UnityPraktikum-Controller


## Ablauf
1. Begrüßung / Instruktion (UI.Text Komponenten; Bewegungssteuerung, weiter per Tastendruck zum testen) (**State**)
2. Übungsphase: Objekt greifen und zum Ausgangspunkt zurückbringen, mit mehr Feedback als im Hauptexperiment
3. Hauptteil
4. Verabschiedung (**State**)

## Datenerfassung
- Verbale Reaktionszeit auf die Vibration
- Bewegungsbeginn, Bewegungsende
- Fehler in der verbalen Reaktion
- (Bewegungsprofil)

## Faktoren
1. Farbe des Zielobjekts (grün = aufrecht greifen; gelb = umgedreht greifen)
2. Stimulation des Fingers (Daumen oder Zeigefinger)
3. Lichtreiz (links oder rechts vom Zielobjekt)
4. Stimulation bei Bewegungsbeginn; nachdem 50% der Strecke zum Ziel zurückgelegt wurden
5. Zehn Wiederholungen

==> 160 Durchgänge Reihenfolge zufällig für jede Versuchsperson

## Zustände für einen Durchgang
1. Initialisierung (**State**) (Auch Faktoren-Liste initialisieren)
   1. nächsten Durchgang einlesen
   2. Objektfarbe bestimmen _(In Variable speichern)_
   3. welcher Finger wird stimuliert _(In Variable speichern)_
   4. Lichtreiz bestimmen _(in Variable speichern)_
2. Vorbereitung
   1. Position des Zielobjekts prüfen _(Abfrage Arduino über Positionierung)_ (**State**)
   2. Handposition auf Ausgangsposition prüfen (Abfragen über Positionierung Kugel m
   deren Farben Stand repräsentiert)
   3. Brille auf undurchsichtig schalten _(Senden an Arduino )_ 
   (**State**)
   4. Farbe des Objekts ändern _(Senden an Arduino)_  (**State**) Ebenfalls darstellen (zum debuggen)
   5. Brille auf sichtbar schalten _(Senden an Arduino)_ (**State**) Ebenfalls darstellen (zum debuggen)
3. Ausführung
   1. Timing der Vibration und des Lichtreizes (beide erscheinen gleichzeitig) _(Senden an Arduino mit Coroutine)_ X Auch mit Update möglich? (**State**)
   2. Kontrolle ob die Handlung durchgeführt wurde; Kontrolle ob verbal reagiert wurde _(Abfragen über Positionierung + Spracherkennung?)_ (**State**)
   3. Datenaufzeichnung _(Gelesene Daten in Variablen speichern)_ ?? spielt in 3.1 und 3.2 bereits mit rein. Extra State unnötig?
   //4. Lichtreize zurücksetzen; taktile Stimulation zurücksetzen _(Senden an Arduino)_ Taktile Stimulation erst jetzt "aus" machen? (**State**)
   5. Zielobjekt zurückstellen + Prüfen _(Abfragen über Positionierung)_ (**State**)
4. Feedback (**State**)
   1. Rückmeldung am Bildschirm
   2. Daten speichern
   3. Test ob das Experiment fertig ist

## Parts Aufteilung
* **Part 1:** Initialisierung, Feedback, Begrüßung + Verabschiedung
* **Part 2:** Vorbereitung, 3.5
* **Part 3:** Ausführung,

## Aufteilung:
* **Marco**: Initialisierung, 2.4, 2.5, 3.1
* **Leonie**: Begrüßung, Verabschiedung, 2.1, 3.2
* **Harald**: Feedback, 2.2, 2.3 , udpmotiontracker
