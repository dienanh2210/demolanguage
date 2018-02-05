//
//  iBeacon.h
//  iBeacon
//
//  Created by anhtu on 1/30/18.
//  Copyright © 2018 anhtu. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <EstimoteSDK/EstimoteSDK.h>

@interface iBeacon : NSObject

+ (iBeacon *)shared;
+ (void)setNotificationText:(NSString *)text;
+ (void)updateAvailableMinorID:(NSString *)minorIDs;
+ (void)updateAvailableMajorID:(NSString *) majorIDs;

@end
