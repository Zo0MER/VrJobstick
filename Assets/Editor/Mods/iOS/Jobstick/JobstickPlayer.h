//
//  JobstickPlayer.h
//  Unity-iPhone
//
//  Created by Office on 20.12.14.
//
//

#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>
#import <CoreBluetooth/CBService.h>


@interface JobstickPlayer : NSObject <CBPeripheralDelegate>
{
}
    @property(copy,readwrite) CBPeripheral     *jobstick;
    @property(assign,readwrite) NSString* address;
    @property(retain, readwrite) NSString* anglessString;

- (instancetype)initWithJobstick:(CBPeripheral *)aJobstick  address:(NSString*) iAddress;

- (NSString *)getAngles;
@end
