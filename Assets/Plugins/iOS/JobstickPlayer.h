//
//
//  Unity-iPhone
//
//  Created by CASE on 01.02.15.
//
//

#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>
#import <CoreBluetooth/CBService.h>


@interface JobstickPlayer : NSObject <CBPeripheralDelegate>
{
}
    @property(readwrite) CBPeripheral     *jobstick;
    @property(readwrite) NSString* address;
    @property(readwrite) NSString* anglessString;

- (instancetype)initWithJobstick:(CBPeripheral *)aJobstick  address:(NSString*) iAddress;

- (NSString *)getAngles;
@end
