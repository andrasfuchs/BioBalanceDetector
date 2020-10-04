from random import getrandbits

def transferFunction(reading):
    def _twosComp(_val):
        if (_val & (1 << 15)) != 0:
            _val = _val - (1 << 16)
        return _val

    
    return (10/2**15)*_twosComp(reading)

_d = bytearray(12)
for i in range(12):
    _d[i] = getrandbits(8)

c1 = _d[0] << 8 | _d[1]
c2 = _d[2] << 8 | _d[3]
c3 = _d[4] << 8 | _d[5]
c4 = _d[6] << 8 | _d[7]
c5 = _d[8] << 8 | _d[9]
c6 = _d[10] << 8 | _d[11]

v1 = transferFunction(c1)
v2 = transferFunction(c2)
v3 = transferFunction(c3)
v4 = transferFunction(c4)
v5 = transferFunction(c5)
v6 = transferFunction(c6)

derp = 1

for _c,_v in zip([c1,c2,c3,c4,c5,c6],[v1,v2,v3,v4,v5,v6]):
    print("Channel {n}\nDecimal: {c}\nBinary: 0b{c:016b}\nHex: 0x{c:04x}\nVoltage: {v}".format(n=derp,c=_c,v=_v))
    derp+=1
