//
//  DBInterface.h
//  Flight Finder
//
//  Created by James Rising on 11/4/13.
//  Copyright (c) 2013 Swift Concepts. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface DBInterface : NSObject

-(NSDictionary*) getObject:(NSString*)name withID:(NSString*)id;
-(NSArray*) getAllObjects:(NSString*)name withOrder:(NSString*)order;
-(NSArray*) getRandomObjects:(NSString*)name limit:(NSInteger)limit;

@end
