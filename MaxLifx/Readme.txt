MaxLifx Release 1

Beta status - known to have minor bugs.  If this blows up your Lifx bulbs or worse, don't blame me.  Total explosions as at 1st November 2015: 0

Release notes

1. Bulb Discovery
=================

Click "Discover" to list your Lifx bulbs.  

* If a Windows Firewall request pops up, you will need to allow it, and then click "Discover" again.
* MaxLifx can only control bulbs which appear in the list above the "Discover" button.

1.1 Click "Turn On All" or "Turn Off All" turn bulbs on or off.  MaxLifx cannot currently turn invidivual bulbs on or off.

1.2 Hit the Panic button to turn all bulbs on, set them to white, and cease any other activity.


2.  Threads
===========

MaxLifx uses individual threads to make things happen.  A threa
d can set bulb colours to match the PC screen, audio playback, or wavefoms.  There is also a thread type which can generate audio.

2.1 Use the three "Start" buttons to start each of the thread types.  You can start more than one of each type of thread, to control bulbs in different ways (for example).  Starting more than one Screen Colour thread may affect performance noticeably.  The different thread types are described below.

2.2 When you start a thread, its user interface (UI) is shown automatically.  If you close this, the thread continues to run.  You can show a thread's UI at any time using the "Show UI" button.

2.3 You can also "Stop Thread" and "End All" threads.

2.4 You can "Load All" and "Save All" threads, which allows you to create and save complex multi-thread scenes.

2.5 You cannot currently rename a thread.


3.  Screen Colour Thread
========================

The Screen Colour Thread sets bulb colour based on an area of your PC's screen.

3.1 To set the screen area, click "Set Screen Area".  A semi-transparent window appears; drag and resize this window as appropriate, then close it.

3.2 Add bulbs by clicking their labels in the list.  You can select multiple bulbs.

3.3 You can assign a different part of the capture area to each bulb using the "Assign Parts of Capture Area..." button. So, for instance, one bulb could match the left side of the capture area, and another bulb the right side.

3.4 You can adjust the following parameters:

* Fade: the time a bulb takes to process a colour change.  Shortening this produces quicker, jerkier effects.
* Delay: the time between bulb updates.  Shorten this for more rapid updates.
* Brightness: the maximum brightness, where 65535 is the highest value allowed
* Saturation: the maximum saturation, where 65535 is the highest value allowed

3.5 You can save your settings, and load them using the drop-down (which currently does not always refresh when it should).


4.  Sound Response Thread
=========================

The Sound Response Thread sets bulb colour based on audio, a waveform (sine/sawtooth/square) or randomly (noise).

4.1 Add bulbs by clicking their labels in the list.  You can select multiple bulbs.

4.2 Drag the handles in the colour selecter to choose a colour and saturation level.  Use the range handles to set a colour range.

4.2 The thread can use a single colour, or one colour per bulb (up to three bulbs).  To use one colour per bulb, click the "Colour Per Bulb" checkbox.

4.3 Invert the colour orders selected by using the "Invert" checkboxes.

4.4 When multiple colours are being selected, you can separate them from each other by using the "Free" checkbox; otherwise, they will move in sympathy.  Similarly, you can set independent ranges for each colour by unchecking the "Link Ranges" checkbox.

4.5 Set the minimum and maximum brightness using the "Brightness Range" sliders.

4.6 Set the Wave Type with the "Wave Type" drop-down:

* Sine: a simple sine wave
* Square: gives a binary on-off effect
* Sawtooth: gradually increases, then instantly drops back to zero
* Audio: uses your Windows audio recording source.  Setting your Windows audio recording source to "Stereo Mix" usually works for audio playback
* Noise: random

4.7 The Wave Duration sets the length of the chosen wave.  For Audio, it has no effect.  For Noise, it determines how often a new random value will be chosen.

4.8 The Delay parameter sets how often bulb colours are updated.  Set this to a lower value for more gradual transitions.

4.9 The Transition parameter sets how long the bulb takes to change colour.  Set this to the same as Delay for smooth gradations, or lower than Delay for stepped gradations.

4.10 The Reset Wave Timer will restart a sine, sawtooth or square wave.  It has no effect for Random or Audio.

4.11 The On and Off Time fields are functional but suboptimal.  Enter a series of semicolon-separated start and stop times, and the thread will react accordingly.  It does not turn bulbs on or off physically.  Note that if you enter any start and stop times, be sure to enter "00:00" as an additional stop time, otherwise activity will automatically restart at that time.

4.12 You can load and save settings as per Screen Colour above.


5.  Sound Generator
===================

The Sound Generator does not control bulbs.  Instead it generates sound, by playing looping sounds and sounds at random intervals.

5.1 Sounds should be stored as WAV files in Documents\MaxLifx\Sounds\Loops and Documents\MaxLifx\Sounds\Random.  When correctly located, they automatically appear in the Sound Generator user interface.  Close and reopen the interface to detect any new sounds.

5.2 Start and stop sounds with the Start/Stop button.  Sounds on the Looping tab loop automatically.

5.3 Sounds on the random tab allow you to set their frequency using the "Average every ... seconds" boxes.

5.4 Each sound can have its own volume level and panning (left-right) position.

5.5 You can load and save settings as per Screen Colour above.

