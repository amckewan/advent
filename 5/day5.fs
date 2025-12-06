\ Day 5

[DEFINED] EMPTY [IF] EMPTY [THEN] MARKER EMPTY DECIMAL
include ../ttester.fs

\ Create list of fresh ranges that we can test
VARIABLE RANGES

: FRESH ( from to -- )
    HERE  RANGES @ ,  RANGES ! ( link)  1+ ( exclusive) , ,  ;

: FRESH? ( n -- f )  RANGES
    BEGIN  @  DUP WHILE
        2DUP CELL+ 2@ WITHIN IF  2DROP TRUE EXIT  THEN
    REPEAT  2DROP FALSE ;

VARIABLE #FRESH

: ID  FRESH? IF  1 #FRESH +!  THEN ;

\ Example data
3 5 FRESH
10 14 FRESH
16 20 FRESH
12 18 FRESH

1 ID
5 ID
8 ID
11 ID
17 ID
32 ID

T{ #FRESH @ -> 3 }T

0 #FRESH !
include input.fs
T{ #FRESH @ -> 577 }T \ right answer
