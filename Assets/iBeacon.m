//
//  iBeacon.m
//  iBeacon
//
//  Created by anhtu on 1/30/18.
//  Copyright © 2018 anhtu. All rights reserved.
//

#import "iBeacon.h"
#import <NotificationCenter/NotificationCenter.h>

#define DEFAULT_UUID  @"B0FC4601-14A6-43A1-ABCD-CB9CFDDB4013"

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

static NSString * KEY_NOTI_TEXT   = @"KEY_NOTI_TEXT";
static NSString * KEY_MINOR_ID        = @"Minor_ID";
static NSString * KEY_MAJOR_ID        = @"Major_ID";
static NSString * KEY_UUID        = @"UUID";
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

+ (void)updateUUID:(NSString *) uuids {
    [[NSUserDefaults standardUserDefaults] setObject: uuids
                                              forKey: KEY_UUID];
    [[iBeacon shared] restart];
}

+ (BOOL)checkExit:(NSString *)uid inKey:(NSString*)key {
    NSString * uidstring = [[NSUserDefaults standardUserDefaults] objectForKey:key];
    NSArray <NSString *> * uuids = [uidstring componentsSeparatedByString:@","];
    if (uuids != nil) {
        for(NSString * uuid in uuids) {
            if ([uuid isEqualToString:uid] && ![uuid isEqualToString:@""]) {
                return YES;
            }
        }
    }
    return NO;
}

+ (BOOL)availableMinor:(NSString *)minorID major:(NSString *)majorID uuid:(NSString *)uuid {
    return ([iBeacon checkExit:minorID inKey:KEY_MINOR_ID] &&
            [iBeacon checkExit:majorID inKey:KEY_MAJOR_ID] &&
            [iBeacon checkExit:uuid inKey:KEY_UUID]);
}

#pragma mark -

- (void)start {
    _beaconRegion = [[CLBeaconRegion alloc] initWithProximityUUID:[[NSUUID alloc]
                                                                   initWithUUIDString:DEFAULT_UUID]
                                                       identifier:@"monitored region"];

    self.beaconManager.delegate = self;
    [self.beaconManager requestAlwaysAuthorization];
    //[self.beaconManager
     //startMonitoringForRegion:_beaconRegion];
    
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

- (void)restart{
    NSString * uuidstring = [[NSUserDefaults standardUserDefaults] objectForKey:KEY_UUID];
    
    [_pushedID removeAllObjects];
    if (uuidstring != nil) {
        NSArray <NSString *> * uuids = [uuidstring componentsSeparatedByString:@","];
        [self.beaconManager stopMonitoringForAllRegions];
        for (NSString * uuid in uuids) {
            if (![uuid isEqualToString:@""]) {
                _beaconRegion = [[CLBeaconRegion alloc]
                                 initWithProximityUUID:[[NSUUID alloc]
                                                        initWithUUIDString:uuid]
                                 identifier:uuid];
                [self.beaconManager
                 startMonitoringForRegion:_beaconRegion];
            }
        }
    }
}

- (void)startRangingItem:(CLBeaconRegion *)region {
    [self.beaconManager startRangingBeaconsInRegion:region];
}

- (void)beaconManager:(id)manager didEnterRegion:(CLBeaconRegion *)region {
    if ([region isKindOfClass:[CLBeaconRegion class]]) {
        [_locationManager startUpdatingLocation];
        [self startRangingItem: region];
    }
}

- (void)beaconManager:(id)manager didExitRegion:(CLBeaconRegion *)region {
    [_beaconManager stopRangingBeaconsInRegion:region];
    
    
    int indexUUID = -1;
    for (int i=0; i<_pushedID.count; i++) {
        NSString * uuid = _pushedID[i];
        if ([uuid isEqualToString:region.proximityUUID.UUIDString]) {
            indexUUID = i;
            break;
        }
    }
    if (indexUUID != -1) {
        [_pushedID removeObjectAtIndex:indexUUID];
    }
}

-(void)beaconManager:(id)manager didRangeBeacons:(NSArray<CLBeacon *> *)beacons inRegion:(CLBeaconRegion *)region {
    NSLog(@"beacon %lu", (unsigned long)beacons.count);
    for (CLBeacon * beacon in beacons) {
        [self sendNotificationIfDetectBeacon:beacon.minor.stringValue
                               major:beacon.major.stringValue
                                uuid:beacon.proximityUUID.UUIDString];
    }
}

- (void)sendNotificationIfDetectBeacon:(NSString *)minorID major:(NSString *) majorID uuid:(NSString *)uuid {
    if ([iBeacon availableMinor:minorID major:majorID uuid:uuid]) {
        [self pushNotification: [NSString stringWithFormat:@"%@%@%@",uuid,minorID,majorID]];
    }
}

- (BOOL)checkPushedId:(NSString *)uid {
    for (NSString * pushedID in _pushedID) {
        if ([pushedID isEqualToString:uid]) {
            return YES;
        }
    }
    
    [_pushedID addObject:uid];
    return NO;
}

- (void)pushNotification: (NSString *)uid {
    
    if ([self checkPushedId:uid]) {
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
