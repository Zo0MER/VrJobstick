//
//  JobstickPlayer.m
//  Unity-iPhone
//
//  Created by Office on 20.12.14.
//
//

#import "JobstickPlayer.h"

// Required by Unity
extern void UnitySendMessage(const char* objectName, const char* methodName, const char* param);

const char* EVENT_MANAGER           = "JobstickEventManager";
const char* onSendAngles            = "OnSendAngles";
const char* onConnectToDevice       = "OnConnectToDevice";

@implementation JobstickPlayer

- (instancetype)initWithJobstick:(CBPeripheral *)aJobstick address:(NSString*) iAddress {
    self = [super init];
    if (self)
    {
        _jobstick = [aJobstick retain];
        _address = iAddress;
        _anglessString = [NSString string];
    }

    return self;
}


- (void) peripheral:(CBPeripheral *)peripheral didDiscoverServices:(NSError *)error
{
    if (error) return;
    
    for (int i=0; i < peripheral.services.count; i++)
    {
        CBService *s = [peripheral.services objectAtIndex:i];
        [peripheral discoverCharacteristics:nil forService:s];
    }
    //UnitySendMessage(EVENT_MANAGER, onConnectToDevice, NULL);
}

- (void)peripheral:(CBPeripheral *)peripheral didDiscoverCharacteristicsForService:(CBService *)service error:(NSError *)error {
    
    if (error) return;
    
    for (int i=0; i < service.characteristics.count; i++)
        [peripheral setNotifyValue:YES forCharacteristic:[service.characteristics objectAtIndex:i]];
    
}

- (void) peripheral:(CBPeripheral*) peripheral didUpdateValueForCharacteristic:(CBCharacteristic *)characteristic error:(NSError *)error {
    
    unsigned char b1[16];
    
    [characteristic.UUID.data getBytes:b1];
    uint16_t characteristicUUID = (b1[0] << 8) | b1[1];
    
    if (error || characteristicUUID != 0xfff4) return;
    
    struct {
        int16_t ax;
        int16_t ay;
        int16_t az;
        int16_t gx;
        int16_t gy;
        int16_t gz;
    } data;
    
    [characteristic.value getBytes:&data length:sizeof(data)];
    [_anglessString release];

    _anglessString = [NSString stringWithFormat:@"%@_%d,%d,%d,%d,%d,%d", _address ,data.ax,data.ay,data.az,data.gx,data.gy,data.gz];

    [_anglessString retain];
    UnitySendMessage(EVENT_MANAGER, onSendAngles, [_anglessString UTF8String]);
}

- (NSString*) getAngles
{
    return _anglessString;
}
@end
