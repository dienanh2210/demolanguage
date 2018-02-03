//
//  iBeacon.m
//  iBeacon
//
//  Created by anhtu on 1/30/18.
//  Copyright © 2018 anhtu. All rights reserved.
//

#import "iBeacon.h"
#import <NotificationCenter/NotificationCenter.h>

@interface iBeacon()<ESTBeaconManagerDelegate, CLLocationManagerDelegate> {
    
}

@property (nonatomic, strong)  ESTBeaconManager * beaconManager;
@property (nonatomic, strong) CLLocationManager * locationManager;
@property (nonatomic, strong) CLBeaconRegion * beaconRegion;
@property (nonatomic, strong) NSMutableArray <NSString *> * pushedID;

@end

@implementation iBeacon

+ (void)load {
    NSLog(@"load iBeacon lib");
}

static NSString * KEY_NOTI_TEXT = @"KEY_NOTI_TEXT";
static NSString * KEY_MINOR_ID        = @"Minor_ID";
static NSString * KEY_MAJOR_ID        = @"Major_ID";
static iBeacon * sharedInstance;

+ (iBeacon *)shared {
    if (sharedInstance == nil) {
        sharedInstance = [iBeacon new];
        sharedInstance.beaconManager =  [ESTBeaconManager new];
        sharedInstance.locationManager = [CLLocationManager new];
        sharedInstance.pushedID = [NSMutableArray new];
        [sharedInstance start];
    }
    return sharedInstance;
}

+ (void)setNotificationText:(NSString *)text {
    [[NSUserDefaults standardUserDefaults] setObject:text
                                              forKey:KEY_NOTI_TEXT];
}

+ (void)updateAvailableMinorID:(NSString *)minorIDs {
    [[NSUserDefaults standardUserDefaults] setObject:minorIDs
                                              forKey:KEY_MINOR_ID];
}

+ (void)updateAvailableMajorID:(NSString *) majorIDs {
    [[NSUserDefaults standardUserDefaults] setObject:majorIDs
                                              forKey:KEY_MAJOR_ID];
}

+ (BOOL)availableMinor:(NSString *)minorID major:(NSString *)majorID {
    
    NSString * minors = [[NSUserDefaults standardUserDefaults] objectForKey: KEY_MINOR_ID];
    NSString * majors = [[NSUserDefaults standardUserDefaults] objectForKey: KEY_MAJOR_ID];
    
    if (minors != nil && majors != nil) {
        NSArray <NSString *> * minorIDs = [minors componentsSeparatedByString:@","];
        NSArray <NSString *> * majorIDs = [majors componentsSeparatedByString:@","];
        
        BOOL isExistMinor = NO;
        BOOL isExistMajor = NO;
        
        for (NSString * minor in minorIDs) {
            if ([minorID isEqualToString:minor]) {
                isExistMinor = YES;
                break;
            }
        }
        for (NSString * major in majorIDs) {
            if ([majorID isEqualToString:major]) {
                isExistMajor = YES;
                break;
            }
        }
        
        return isExistMinor && isExistMajor;
    }
    
    return NO;
}

#pragma mark -

- (void)start {
 
    _beaconRegion = [[CLBeaconRegion alloc]
initWithProximityUUID:[[NSUUID alloc]
                       initWithUUIDString:@"B0FC4601-14A6-43A1-ABCD-CB9CFDDB4013"]
identifier:@"monitored region"];

    self.beaconManager.delegate = self;
    [self.beaconManager requestAlwaysAuthorization];
    [self.beaconManager
     startMonitoringForRegion:_beaconRegion];
    
    [[UIApplication sharedApplication]
     registerUserNotificationSettings:
     [UIUserNotificationSettings
      settingsForTypes:UIUserNotificationTypeAlert|UIUserNotificationTypeSound
      categories:nil]];
    
    self.locationManager.desiredAccuracy = kCLLocationAccuracyThreeKilometers;
    self.locationManager.allowsBackgroundLocationUpdates = true;
    [self.locationManager startMonitoringSignificantLocationChanges];
    [self.locationManager requestAlwaysAuthorization];
    [_pushedID removeAllObjects];
}

- (void)startRangingItem {
    [_pushedID removeAllObjects];
    [self.beaconManager stopRangingBeaconsInAllRegions];
    [self.beaconManager startRangingBeaconsInRegion:_beaconRegion];
}

- (void)beaconManager:(id)manager didEnterRegion:(CLBeaconRegion *)region {
    if ([region isKindOfClass:[CLBeaconRegion class]]) {
        [_locationManager startUpdatingLocation];
        [self startRangingItem];
    }
}

- (void)beaconManager:(id)manager didExitRegion:(CLBeaconRegion *)region {
    [_locationManager stopUpdatingLocation];
    [_beaconManager stopRangingBeaconsInRegion:_beaconRegion];
    [_pushedID removeAllObjects];
}

-(void)beaconManager:(id)manager didRangeBeacons:(NSArray<CLBeacon *> *)beacons inRegion:(CLBeaconRegion *)region {
    NSLog(@"beacon %lu", (unsigned long)beacons.count);
    for (CLBeacon * beacon in beacons) {
        [self checkMinorAndPushMinor:beacon.minor.stringValue major:beacon.major.stringValue];
    }
}

- (void)checkMinorAndPushMinor:(NSString *)minorID major:(NSString *) majorID {
    if ([iBeacon availableMinor:minorID major:majorID]) {
        [self pushNotification:minorID];
    }
}

- (BOOL)checkPushedId:(NSString *)minorID {
    for (NSString * pushedID in _pushedID) {
        if ([pushedID isEqualToString:minorID]) {
            return YES;
        }
    }
    
    [_pushedID addObject:minorID];
    return NO;
}

- (void)pushNotification: (NSString *)minorID {
    
    if ([self checkPushedId:minorID]) {
        return;
    }
    
    UILocalNotification *notification = [UILocalNotification new];
    NSString * body = @"Yeah ! You see a Yokai.. Do you wanna catch it ?";
    NSString * cusText = [[NSUserDefaults standardUserDefaults] objectForKey:KEY_NOTI_TEXT];
    if (cusText != nil && ![cusText isEqualToString:@""]) {
        body = cusText;
    }
    notification.alertBody = body;
    notification.soundName = UILocalNotificationDefaultSoundName;
    [[UIApplication sharedApplication] presentLocalNotificationNow:notification];
}

@end
