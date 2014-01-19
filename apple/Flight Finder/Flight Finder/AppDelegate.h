//
//  AppDelegate.h
//  Flight Finder
//
//  Created by James Rising on 11/7/13.
//  Copyright (c) 2013 Swift Concepts. All rights reserved.
//

#import <UIKit/UIKit.h>

#import "DBInterface.h"
#import "StatusManager.h"

@interface AppDelegate : UIResponder <UIApplicationDelegate> {

@public
    DBInterface* dbface;
    StatusManager* status;
    
}

@property (strong, nonatomic) UIWindow *window;
@property (nonatomic, retain) DBInterface *dbface;
@property (nonatomic, retain) StatusManager* status;

@property (readonly, strong, nonatomic) NSManagedObjectContext *managedObjectContext;
@property (readonly, strong, nonatomic) NSManagedObjectModel *managedObjectModel;
@property (readonly, strong, nonatomic) NSPersistentStoreCoordinator *persistentStoreCoordinator;

- (void)saveContext;
- (NSURL *)applicationDocumentsDirectory;

// Helper to get AppDelegate
+(AppDelegate*) getAppDelegate;

@end
