//
//  DBInterface.m
//  Flight Finder
//
//  Created by James Rising on 11/4/13.
//  Copyright (c) 2013 Swift Concepts. All rights reserved.
//

#import "DBInterface.h"

@implementation DBInterface

-(NSDictionary*) getObject:(NSString*)name withID:(NSString*)id {
    //List<Dictionary<string, object>> rows = dbface.AssocEnumerate(string.Format("select * from {0} where code = '{1}' limit 1", name, id));
    return [NSDictionary dictionary];
}

-(NSArray*) getAllObjects:(NSString*)name withOrder:(NSString *)order {
    /*string sql = "select * from airports";
    if (!string.IsNullOrEmpty(order))
        sql += " order by " + order;*/
    return [NSArray array];
}


-(NSArray*) getRandomObjects:(NSString*)name limit:(NSInteger)limit {
    //    List<Dictionary<string, object>> rows = dbface.AssocEnumerate("select * from airports order by salience * rand() desc limit " + (countDisabled + 1));
    return [NSArray array];
}

@end
