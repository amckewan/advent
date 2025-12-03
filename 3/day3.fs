\ Day 3

[DEFINED] EMPTY [IF] EMPTY [THEN] MARKER EMPTY
include ../ttester.fs

: |   BL WORD C@ 1+ ALLOT ;

CREATE EXAMPLE
| 987654321111111
| 811111111111119
| 234234234234278
| 818181911112111
|

include input.fs

: LARGEST ( a n -- a2 n2 )
    SWAP 2DUP DUP ROT ( n a a a n ) BOUNDS DO ( n a a' )
        I C@ OVER C@ > IF  DROP I  THEN
    LOOP  DUP ROT -  ROT SWAP - ;

T{ EXAMPLE      COUNT           LARGEST SWAP C@ -> 15 '9' }T
T{ EXAMPLE 16 + COUNT 1-        LARGEST SWAP C@ -> 14 '8' }T
T{ EXAMPLE 16 + COUNT 1 /STRING LARGEST SWAP C@ ->  1 '9' }T

: BANK ( a n -- jolts )
    1- LARGEST  OVER C@ '0' - >R  1 /STRING
    1+ LARGEST  DROP C@ '0' - R> 10 * + ;

: JOLTS ( input -- jolts )  0 SWAP
   BEGIN  DUP COUNT BANK  ROT + SWAP
     COUNT +  DUP C@ 0= UNTIL  DROP ;

T{ EXAMPLE JOLTS -> 357 }T
T{ INPUT JOLTS -> 17435 }T
