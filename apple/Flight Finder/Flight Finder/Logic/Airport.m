//
//  Airport.m
//  Flight Finder
//
//  Created by James Rising on 11/4/13.
//  Copyright (c) 2013 Swift Concepts. All rights reserved.
//

#import "Airport.h"
#import "AppDelegate.h"

@interface Airport()

// Single airport objects for each known airport
+(NSMutableDictionary*) cache;
// How many airports are currently disabled (maintained by IsEnabled)
+(int) countDisabled;

@property NSString* code;    // three letter airport code
@property NSString* title;   // a title for the airport
@property NSString* country; // the country where it is

@property bool enabled;      // Is this airport available for a destination?

@end

@implementation Airport

@synthesize code, title, country;

/***** Static Methods *****/

+(NSMutableDictionary*) cache {
    static NSMutableDictionary* cache = nil;
    
    @synchronized(self) {
        if (cache == nil)
            cache = [NSMutableDictionary dictionary];
        
        return cache;
    }
}

static int countDisabled = 0;
+ (int) countDisabled {
    @synchronized(self) {
        return countDisabled;
    }
}
+ (void) setCountDisabled:(int)value {
    @synchronized(self) {
        countDisabled = value;
    }
}

+(Airport*) getAirport:(NSString*)code {
    @synchronized(self.cache)
    {
        // look in the cache
        Airport* airport = [self.cache objectForKey:code];
        if (airport != nil)
            return airport;
        
        // look in the database
        NSDictionary* row = [AppDelegate.getAppDelegate.dbface getObject:@"airports" withID: code];
        if (row == nil)
            return [[Airport alloc] init:code withTitle:@"Unknown" withCountry:@"Unknown"];
        
        airport = [[Airport alloc] init:[row objectForKey:@"code"] withTitle:[row objectForKey:@"title"] withCountry:[row objectForKey:@"country"]];
        [self.cache setObject:airport forKey:code];
        
        return airport;
    }
}

// Remember the information for an airport
+(Airport*) recordAirport:(NSString*)code withTitle:(NSString*)title withCountry:(NSString*)country
{
    @synchronized(self.cache)
    {
        // look in the cache
        Airport* airport = [self.cache objectForKey:code];
        if (airport)
            return airport;
        
        // Add it to the cache
        airport = [[Airport alloc] init:code withTitle:title withCountry:country];
        [self.cache setObject:airport forKey:code];
        
        return airport;
    }
}

// Remember the information for an airport
+(Airport*) recordAirportRow:(NSDictionary*)row
{
    return [self recordAirport:[row objectForKey:@"code"] withTitle:[row objectForKey:@"title"] withCountry:[row objectForKey:@"country"]];
}

// Get a random airport, based on salience.  Defaults to BOS, if there's an error.
+(NSString*) getSalientAirportCode
{
    NSArray* rows = [AppDelegate.getAppDelegate.dbface getRandomObjects:@"airports" limit:(countDisabled + 1)];
    if (rows == nil)
        return @"BOS";
    
    for (int ii = 0; ii < rows.count; ii++) {
        NSDictionary* row = [rows objectAtIndex:ii];
        Airport* airport = [self recordAirportRow:row];
        if (!airport.enabled)
            continue;
            
        return airport.code;
    }
        
    return @"BOS";
}

+(NSArray*) loadAllAirports:(NSString*)order
{
    NSArray* rows = [AppDelegate.getAppDelegate.dbface getAllObjects:@"airports" withOrder:order];
    if (rows == nil)
        return nil; // failed!
    
    NSMutableArray* airports = [NSMutableArray array];
    for (int ii = 0; ii < rows.count; ii++) {
        NSDictionary* row = [rows objectAtIndex:ii];
        [airports addObject:[self recordAirportRow:row]];
    }
    
    return airports;
}

// Is this airport enabled?  It is if we don't know otherwise.
+(bool) isAirportEnabled:(NSString*)code {
    @synchronized(self.cache)
    {
        // look in the cache
        Airport* airport = [self.cache objectForKey:code];
        if (airport)
            return airport.enabled;
        
        return true;    // it's never been disabled
    }
}

// Get all the airports that are currently disabled
+(NSString*) getDisabledCodes
{
    NSMutableArray* codes = [NSMutableArray array];
    
    for (NSString* code in self.cache)
        if (((Airport*)[self.cache objectForKey:code]).enabled)
            [codes addObject:code];
    
    return [codes componentsJoinedByString:@", "];
}

// Disable the airports described in this list
+(void) disableCodes:(NSString*)list
{
    if (list == nil)
        return;
    
    
    NSArray* codes = [self parseAirportCodes:list];
    if (codes.count > 20)
    {
        // First cache all airports
        [self loadAllAirports:nil];
    }
    
    for (NSString* code in codes)
        [self getAirport:code].enabled = false;
}

// Split up a list of airport codes
+(NSArray*) parseAirportCodes:(NSString*)list
{
    return [list componentsSeparatedByCharactersInSet:[NSCharacterSet characterSetWithCharactersInString:@" ,;\'"]];
}

/***** Airport Instance *****/

// Protected constructor-- all Airports are gotten through GetAirport or RecordAirport
-(id) init:(NSString*)cod withTitle:(NSString*)titl withCountry:(NSString*)countr
{
    if (self = [super init]) {
        self.code = cod;
        self.title = titl;
        self.country = countr;
        
        self.enabled = true;
    }
    
    return self;
}

// is this airport available in the search?
-(bool) enabled
{
    return self.enabled;
}

-(void) setEnabled:(bool)value
{
    // maintain the countDisabled value
    if (self.enabled != value) {
        if (self.enabled)
            countDisabled++;
        else
            countDisabled--;
    }
    self.enabled = value;
}

// A NSString* describing all relevant information
-(NSString*) description
{
    return [NSString stringWithFormat: @"%@, %@ (%@)", title, country, code];
}

@end
