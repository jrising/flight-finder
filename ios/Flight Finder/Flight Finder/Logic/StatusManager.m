/******************************************************************************\
 * StatusManager - threadsafe manager of the status and progress bars
 * ----------------------------------------------------------------------------
 * Copyright (C) 2009  James Rising
 *
 * Licensed under the GNU General Public License, Version 3 (see license.txt)
 \******************************************************************************/

#import "StatusManager.h"

@interface StatusManager()

/*
@property StatusStrip strip;
@property ToolStripStatusLabel label;
@property ToolStripProgressBar progress;

@property Queue<string> messages;
@property int diffTodo;
@property int diffDone;
@property Stopwatch watch;
*/
@end

@implementation StatusManager

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        // Initialization code
    }
    return self;
}

-(void)	addNow:(NSString*)message withMax:(NSInteger)max withValue:(NSInteger)value {
    // TODO: implement
}
    
/*        public StatusManager(StatusStrip strip, ToolStripStatusLabel label, ToolStripProgressBar progress)
        {
            this.strip = strip;
            this.label = label;
            this.progress = progress;
            
            label.Text = "Initializing...";
            watch = new Stopwatch();
            watch.Start();
            progress.Minimum = 0;
            progress.Step = 1;
            progress.Maximum = 1;
            
            messages = new Queue<string>();
        }
        
        public void AddNow(string message, int max, int value)
        {
            lock (this)
            {
                label.Text = message;
                progress.Style = ProgressBarStyle.Blocks;
                progress.Maximum = max;
                progress.Value = value;
                messages.Clear();
            }
        }
        
        public void EnterContinuousMode()
        {
            progress.Style = ProgressBarStyle.Marquee;
        }
        
        public void Add(string message)
        {
            lock (this)
            {
                messages.Enqueue(message);
            }
			Log.Info(message);
        }
        
        public void Update()
        {
            lock (this)
            {
                if (messages.Count > 0)
                {
                    if (watch.ElapsedMilliseconds > 1000 / messages.Count)
                    {
                        label.Text = messages.Dequeue();
                        watch.Reset();
                        watch.Start();
                    }
                }
            }
        }
*/

@end
