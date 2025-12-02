\ Advent of Code 2025, day 1
\ Andrew McKewan, andrew.mckewan@gmail.com

\ Open the safe
VARIABLE DIAL
VARIABLE DIRECTION
VARIABLE ZEROS

: START ( -- 0 )  50 DIAL !  0 DIRECTION !  0 ZEROS !  0 ;

: ?ZERO ( n -- )  0= NEGATE ZEROS +! ;


MARKER PART  ( === Part 1 === )

: TURN ( n -- )
    DIRECTION @ *  DIAL @ +  100 MOD  DUP DIAL !  ?ZERO ;

: L ( n -- )  TURN  -1 DIRECTION ! ;
: R ( n -- )  TURN   1 DIRECTION ! ;

: END ( n -- )   TURN   CR ." Part 1: " ZEROS ? ;

INCLUDE data.fs


PART  ( === Part 2 === )

: CLICK ( -- )
    DIRECTION @  DIAL @ +  100 MOD  DUP DIAL !  ?ZERO ;
: TURN ( n -- )
    0 ?DO  CLICK  LOOP ;

: L ( n -- )  TURN  -1 DIRECTION ! ;
: R ( n -- )  TURN   1 DIRECTION ! ;

: END ( n -- )   TURN   CR ." Part 2: " ZEROS ? CR ;

INCLUDE data.fs
