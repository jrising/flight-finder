/******************************************************************************\
 * OriginFinder - window for searching for airports and adding to origins
 * ----------------------------------------------------------------------------
 * Copyright (C) 2009, 2013  James Rising
 *
 * Licensed under the GNU General Public License, Version 3 (see license.txt)
 \******************************************************************************/

#import "AppDelegate.h"
#import "AirportFinder.h"
#import "Airport.h"

@interface AirportFinder() {

    UITextField* origins;
    NSArray* airports;
    NSMutableDictionary* catalog;
}

@end

@implementation AirportFinder

/*+(void) displayPopover:(UIView*)anchor withViewController:(UIViewController*)controller {
    UIPopoverController* popover = [[UIPopoverController alloc] initWithContentViewController:controller];
    [popover presentPopoverFromRect:anchor.frame inView:controller.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
}

- (id)initWithFrame:(CGRect)frame withOrigins:(UITextField*)orgs
{
    self = [super initWithFrame:frame];
    if (self) {
        self->origins = orgs;
                    
        // Collect All Airports
        airports = [Airport loadAllAirports:@"country asc"];
        if (airports == nil) {
            AppDelegate *appDelegate = (AppDelegate *)[[UIApplication sharedApplication] delegate];
            [appDelegate.status addNow:@"Could not access airport information" withMax:0 withValue:0];
            [self dismiss];
            return self;
        }
            
        [searchField addTarget:self action:@selector(searchFieldDidChange:) forControlEvents:UIControlEventEditingChanged];
        
        catalog = [NSMutableDictionary dictionary];
            
        // Display all airports
        for (Airport* airport in airports) {
            string line = airport.ToString();
            catalog.Add(line.ToLower(), airport);
            lstAirports.Items.Add(airport);
        }
    }

    return self;
}

-(void) searchFieldDidChange:(id)sender
{
    string text = searchField.text.ToLower();
    
    // Search all airports
    NSArray* matches = new NSArray*();
    foreach (KeyValuePair<string, Airport> entry in catalog)
    if (entry.Key.Contains(text))
        matches.Add(entry.Value);
    
    lstAirports.Items.Clear();
    lstAirports.Items.AddRange(matches.ToArray());
}
*/

@end

/*
		// On idle, do more filtering options
        private void OnIdle(object sender, EventArgs e)
        {
			if (idleEnumerator == null)
				return;
			
            string text = txtSearch.Text.ToLower();
			
			int added = 0;
			while (idleEnumerator != null && added < 2) {
				if (!idleEnumerator.Value.MoveNext())
					idleEnumerator = null;
				else {
					KeyValuePair<string, Airport> entry = idleEnumerator.Value.Current;
	                if (entry.Key.Contains(text)) {
    	                lstAirports.Items.Add(entry.Value);
						added++;
					}
				}
            }
		}
        
        
        private void lstAirports_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clean up and add it to the list
            string[] codes = Airport.ParseAirportCodes(origins.Text);
            
            // Is self already in our list?
            Airport selected = (Airport)lstAirports.SelectedItem;
            foreach (string code in codes)
            if (code == selected.Code)
                return;
            
			if (codes.Length == 0)
		        origins.Text = selected.Code;
			else
	            origins.Text = string.Join(", ", codes) + ", " + selected.Code;
        }
        
        private void btnDone_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
*/