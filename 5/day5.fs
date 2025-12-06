\ Day 5

[DEFINED] EMPTY [IF] EMPTY [THEN] MARKER EMPTY
CLEARSTACK DECIMAL
include ../ttester.fs


VARIABLE RANGES     \ List of ID ranges:  | to+1 | from | 'next |

: RANGE, ( from to -- )  ALIGN HERE >R   1+ ,  ,  RANGES @ ,   R> RANGES ! ;

: NEXT ( range -- range|0 )  CELL+ CELL+ @ ;

: .RANGE ( range -- )  2@  OVER . ." - " DUP .  ." (" SWAP - 0 .R ." ) " ;
: .RANGES   RANGES @  BEGIN ?DUP WHILE  DUP CR .RANGE  NEXT  REPEAT ;

: FRESH? ( id -- n )  >R  0  RANGES @
    BEGIN ?DUP WHILE
        R@ OVER 2@ WITHIN IF  SWAP 1+ SWAP  THEN  NEXT
    REPEAT  R> DROP ;

RANGES OFF
100 200 RANGE,
300 400 RANGE,
500 600 RANGE,
550 650 RANGE,

T{ 100 FRESH? -> 1 }T
T{ 350 FRESH? -> 1 }T
T{ 575 FRESH? -> 2 }T
T{  50 FRESH? -> 0 }T
T{ 250 FRESH? -> 0 }T
T{ 850 FRESH? -> 0 }T


VARIABLE #FRESH
: START  RANGES OFF  #FRESH OFF ;
: ID ( n -- )  FRESH? IF  1 #FRESH +!  THEN ;


: OVERLAP ( range1 range2 -- n )  2@ ROT 2@ ( from2 to2 from1 to1 )
    ROT MIN ( to)  -ROT MAX ( from) -   0 MAX ;

: JOIN ( range r2 -- )   2DUP 2>R
    2@ ROT 2@  ROT MAX ( to)  -ROT MIN ( from)  SWAP
    0 0 R> 2!  R> 2! ;

\ Join all following overlapping ranges into this one.
: JOIN-RANGE ( range -- )  DUP
    BEGIN  NEXT  ?DUP WHILE
        2DUP OVERLAP IF  2DUP JOIN  THEN
    REPEAT DROP ;

: TOTAL ( -- n )  0  RANGES @
    BEGIN ?DUP WHILE
        DUP JOIN-RANGE  DUP 2@ SWAP -  ROT + SWAP  NEXT
    REPEAT ;



\ Example data
START
3 5 RANGE,
10 14 RANGE,
16 20 RANGE,
12 18 RANGE,

1 ID
5 ID
8 ID
11 ID
17 ID
32 ID

T{ #FRESH @ -> 3 }T
T{ TOTAL -> 14 }T

: INPUT  START  S" input.fs" INCLUDED ;

INPUT
T{ #FRESH @ -> 577 }T
T{ TOTAL -> 350513176552950 }T
\           353631645572217
\           349881887868540 too low

