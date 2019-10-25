# MaxLifx-Z

MaxLifx-Z is a fork of the original MaxLifx with one goal in mind: get each of zones in a multizone light (such the Lifx Z), to respond to a certain area of the screen. In other words, an ambilight for your monitor using Lifx Z.

Grab the latest release here: https://github.com/gitCommitWiL/MaxLifx-Z/releases

## v1.1: What's new?

- Added multizone support
- Added Kelvin parameter
- Changed colours from HSL to HSV (in general, colours will be brighter)


## Ambilight Setup

After starting a screen colour thread, select your multizone bulb from the list.

Click on 'Set Screen Area'. Your selected multizone should respond by turning off all zones, except the first starting zone which should now be green. We will be using this zone to calculate the location for the rest of the zones.

Move and resize the window to approximately where you want the green zone to react to on screen. Close the window when finished.
- You can also fine tune this area by changing the Top-Left and Bottom-Right values in the ScreenColour window.

Click on 'Assign Parts of Capture Area To Bulbs' and select your multizone light from the list.

By default, the area assigned to your multizone light should be set to 'None'. Change it to either SurroundClockwise, or SurroundCounterClockwise, depending on how you set your lights up. Your multizone light should now be responding to your screen.


## Demos

The following demos show off MaxLifx-Z. The effect is a little difficult to capture on video; it's much more noticeable in person.
Also, there is no wall behind the monitor, thus making the effect in this video a little more difficult to see.

[![Demo MaxLifx-Z](Media/Demo1.gif)](https://youtu.be/yK4OV-j1sws)


[![Demo MaxLifx-Z Rocket League](Media/Demo2.gif)](https://youtu.be/1kSUHLQzJ50)


Credits to /u/thmstec
[![Demo MaxLifx-Z thmstec](Media/DemoThmstec.gif)](https://imgur.com/a/wHRuaDM)

## Accuracy

In order to get a more accurate setup, you may need to fine tune the selected screen area. Since all other zones are calculated based on the initial selected area, there may be a couple of different factors that may be causing the inaccuracy of other zones:
- the thickness of the bezel around your monitor
- whether or not your monitor is curved
- the starting point of the inital zone
- how you set up the light around the monitor

The "ideal" measurement for the initial zone would be at the start of the strip to the 0.3cm past the last led in the first zone since there is about 1.3cm of space from the start of the strip to first led and about 1.6cm space between last led in zone and first led in new zone (of same strip).
The distance between last led in strip and first led in next strip is about 1.9cm. But this "ideal" measurement assumes a perfectly flat monitor with no bezel; basically just the panel.

In any case, you may want to test which zones are responding to which screen area by dragging a coloured box around a blank canvas in Microsoft paint or something similar to that.
Note: for the first zone to turn green and all other zone turn off, your light area must be assigned to 'None' before clicking on 'Set Screen Area'


## Troubleshooting

If your multizone light is not recognised as multizone capable, then you may have to rediscover your lights. A good indicator of this is when you select your multizone light for the first time from 'Assign Parts of Capture Area To Bulbs' and it is set by default to 'All' instead of 'None'.
Another indicator of this is if the application does not update to use your light's name and instead uses the serial number.

If you're having trouble discovering your bulbs, one suggestion is to be on the same network frequency band. I've had trouble discovering my lights a couple of times if my computer wasn't on the same 2.4 Ghz band that my bulbs were on, and instead was on the 5 Ghz band.

By default, the delay is set to 50ms. I've found that decreasing the delay by too much actually made the responsiveness worse for me, but YMMV.


## Remarks

- If MaxLifx wasn't working previously for you, then v1.1 of MaxLifx-Z will likely not work too; you can still give it a try but none of the network discovery code was really changed.
- If you don't have any multizone lights you may still want to check out MaxLifx-Z; there are a few quality of life changes you may find useful.
- Your saved threads from MaxLifx should carry over, but it's not guaranteed. Still worth a shot though.
- Although I don't have one to test with, Lifx Beam lights are mutlizone and technically should work... but I can't guarantee the accuracy. I'm not sure if the corners affect the calculations of the zones in any way so an accurate setup might be more difficult. Might look cool if you use a TV as a monitor though.
- However, Lifx Tiles will not work since they do not use the Lifx multizone api.


## For additional information on how to use MaxLifx-Z, the original readme from MaxLifx is below:

MaxLifx v0.2

https://github.com/stringandstickytape/MaxLifx

Beta status - known to have minor bugs.  If this blows up your Lifx bulbs or worse, don't blame me.  Total explosions as at 12th December 2015: 0

Release notes

1. Bulb Discovery
=================

Click "Discover" to list your Lifx bulbs.  You must have labelled them and updated their firmware using Android/Windows/iOS first.

* If a Windows Firewall request pops up, you will need to allow it, and then click "Discover" again.
* MaxLifx can only control bulbs which appear in the list above the "Discover" button.

1.1 Click "Turn On All" or "Turn Off All" turn bulbs on or off.  MaxLifx cannot currently turn individual bulbs on or off.

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

4.13 (New in 0.2) For the Audio wave type, you can specify the frequency response range for each bulb independently using the Audio Frequency Response pane.  

4.14 (New in 0.2) You can reorder the bulbs for the sake of convenience.  Click "Reorder", then use the "Up" and "Down" buttons.  When you're done, click "Reorder" again.

5.  Sound Generator
===================

The Sound Generator does not control bulbs.  Instead it generates sound, by playing looping sounds and sounds at random intervals.

5.1 Sounds should be stored as WAV files in Documents\MaxLifx\Sounds\Loops and Documents\MaxLifx\Sounds\Random.  When correctly located, they automatically appear in the Sound Generator user interface.  Close and reopen the interface to detect any new sounds.

5.2 Start and stop sounds with the Start/Stop button.  Sounds on the Looping tab loop automatically.

5.3 Sounds on the random tab allow you to set their frequency using the "Average every ... seconds" boxes.

5.4 Each sound can have its own volume level and panning (left-right) position.

5.5 You can load and save settings as per Screen Colour above.

6. (New in 0.2) Bulb Monitors
=============================

Bulb monitors show the most recent colour messages sent to each bulb.  Click the button under the monitor to pop a window for that bulb's colour.

6.1 Click the "[+]" button to hide the Bulb Monitors pane.

7. (New in 0.2, experimental) Threadset / MP3 Sequencer
=======================================================

The Sequencer allows you to synchronize MP3 playback with bulb effects.  You should save regularly, because the sequencer is experimental and has bugs.

7.1 Click "Add" to add an event.

7.2 Select the event and click "Edit" to set its type - PlayMp3 or StartThreadSet.  Use the Browse button to find an MP3 or ThreadSet, as appropriate.

7.3 MP3s are shown in the timeline as a waveform.  Starting a second MP3 will kill playback of the first.

7.4 You can drag events to change their time.  SHIFT-dragging will create a copy of the event.  CTRL-clicking allows you to select and edit multiple events simultaneously.

7.5 To be clear, a ThreadSet is a collection of one or more threads, created using the "Start ... Thread" buttons and then saved as a ThreadSet using the Save ThreadSet button.  These ThreadSets can then be sequenced.  Typically, a ThreadSet would only have one thread, but there is no limitation.

7.6 Click the "[+]" button to hide the Sequencer pane.
