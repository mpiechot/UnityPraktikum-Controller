# UnityPraktikum-Controller


## Ablauf
1. Begrüßung / Instruktion (UI.Text Komponenten; Bewegungssteuerung, weiter per Tastendruck zum testen)
2. Übungsphase: Objekt greifen und zum Ausgangspunkt zurückbringen, mit mehr Feedback als im Hauptexperiment
3. Hauptteil
4. Verabschiedung

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
1. Initialisierung
   1. nächsten Durchgang einlesen
   2. Objektfarbe bestimmen _(In Variable speichern)_
   3. welcher Finger wird stimuliert _(In Variable speichern)_
   4. Lichtreiz bestimmen _(in Variable speichern)_
2. Vorbereitung
   1. Position des Zielobjekts prüfen _(Abfrage Arduino über Positionierung)_
   2. Brille auf undurchsichtig schalten + Handposition prüfen _(Senden an Arduino + Abfragen über Positionierung)_
   3. Farbe des Objekts ändern _(Senden an Arduino)_
   4. Brille auf sichtbar schalten _(Senden an Arduino)_
3. Ausführung
   1. Timing der Vibration und des Lichtreizes (beide erscheinen gleichzeitig) _(Senden an Arduino mit Coroutine)_
   2. Kontrolle ob die Handlung durchgeführt wurde; Kontrolle ob verbal reagiert wurde _(Abfragen über Positionierung + Spracherkennung?)_
   3. Datenaufzeichnung _(Gelesene Daten in Variablen speichern)_
   4. Lichtreize zurücksetzen; taktile Stimulation zurücksetzen _(Senden an Arduino)_
   5. Zielobjekt zurückstellen + Prüfen _(Senden an Arduino + Abfragen über Positionierung)_
4. Feedback
   1. Rückmeldung am Bildschirm
   2. Daten speichern
   3. Test ob das Experiment fertig ist
