\ Day 8, part 1

include ../init.fs

0 VALUE BOXES
0 VALUE #BOXES

: BOX ( box# -- a )  3 CELLS * BOXES + ;

: X   ( box -- x )   @ ;
: Y   ( box -- y )   CELL + @ ;
: Z   ( box -- z )   2 CELLS + @ ;

: X!   ( x box -- )   ! ;
: Y!   ( y box -- )   CELL + ! ;
: Z!   ( z box -- )   2 CELLS + ! ;

: .BOX ( box# )  BOX  DUP X 0 .R  ." ,"  DUP Y 0 .R  ." ,"  Z . ;

: DIST ( b1 b2 -- n ) \ sqrt((x2-x1)^2 + (y2-y1)^2 + (z2-z1)^2)
    OVER BOX X  OVER BOX X -  DUP *  >R
    OVER BOX Y  OVER BOX Y -  DUP *  >R
         BOX Z  SWAP BOX Z -  DUP *  2R> + +
    S>F FSQRT F>S ;

0 VALUE DISTANCES
: 'DIST ( b1 b2 -- a )  #BOXES * +  CELLS DISTANCES + ;
: PRECALC ( -- ) \ pre-calculate distances
    HERE TO DISTANCES  #BOXES DUP * CELLS ALLOT
    #BOXES 0 DO  #BOXES 0 DO  I J DIST  I J 'DIST !  LOOP LOOP ;
: DISTANCE  ( b1 b2 -- n )  'DIST @ ;

: .DISTANCES
    #BOXES 0 DO  CR #BOXES 0 DO  I J DISTANCE 5 .R  LOOP LOOP ;

: :BOXES  ALIGN  HERE TO BOXES ;
: ;BOXES  HERE BOXES - 3 CELLS / TO #BOXES  PRECALC ;

: EXAMPLE
    :BOXES
    162  ,  817  ,  812 , 
    57  ,  618  ,  57 , 
    906  ,  360  ,  560 , 
    592  ,  479  ,  940 , 
    352  ,  342  ,  300 , 
    466  ,  668  ,  158 , 
    542  ,  29  ,  236 , 
    431  ,  825  ,  988 , 
    739 , 650 , 466 , 
    52 , 470 , 668 , 
    216 , 146 , 977 , 
    819 , 987 , 18 , 
    117 , 168 , 530 , 
    805 , 96 , 715 , 
    346 , 949 , 466 , 
    970 , 615 , 88 , 
    941 , 993 , 340 , 
    862 , 61 , 35 , 
    984 , 92 , 344 , 
    425 , 690 , 689 , 
    ;BOXES ;
EXAMPLE
T{ 1 3 DISTANCE -> 1 3 DIST }T

: INPUT  :BOXES  S" input.fs" INCLUDED  ;BOXES ;

: ZEROS  0  #BOXES 0 DO  #BOXES 0 DO  J I DISTANCE 0= IF 1+ THEN  LOOP LOOP ;


: SHORTEST ( floor -- b1 b2 )   0 0 ROT 1+ -1 ( b1 b2 floor+1 shortest )
    #BOXES 0 DO  #BOXES 0 DO ( 1,000,000 times )
        2DUP I J DISTANCE -ROT WITHIN ( > floor and < shortest )
        IF  DROP NIP NIP  I J  ROT  I J DISTANCE  THEN
    LOOP LOOP 2DROP ;

: .() ( n )  ."   (" 0 .R ." ) " ;
: L>H ( b1 b2 -- low high )  2DUP > IF SWAP THEN ;


VARIABLE CONNECTIONS
: LINK ( a -- )  HERE  OVER @ ,  SWAP ! ;
: CONNECT ( b1 b2 -- )  CONNECTIONS LINK  L>H , ,  0 , ( circuit ) ;
: 'CIRCUIT ( conn -- n )   3 CELLS + ;
: MAKE ( n )  CONNECTIONS OFF  \ Make n shortest connections
    0 ( floor)  SWAP 0 DO  SHORTEST  2DUP CONNECT  DISTANCE  LOOP  DROP ;

: C CONNECT ;
: .CONN ( conn )  DUP 'CIRCUIT @ .()
    CELL+ 2@  2DUP SWAP 3 .R ."  - " 3 .R  DISTANCE .() ;
: CC   CONNECTIONS  BEGIN @ ?DUP WHILE CR DUP .CONN REPEAT ;

VARIABLE CIRCUITS
: CIRCUIT ( conn )
    1 CIRCUITS +!  CIRCUITS @ SWAP 'CIRCUIT ! 
    
    BEGIN @ ?DUP WHILE
        ( does this one belong? )
    REPEAT ;

10 MAKE
CC

quit

\ Connection:   link, b1, b2
\ Circuit:      link, connections
VARIABLE CIRCUITS

: CIRC@ ( circuit -- conn )      CELL+  @ ;
: CONN@ ( connection -- b1 b2 )  CELL+ 2@ ;

: .CIRCUIT ( circ )  CR ." Circuit:"  CIRC@
    BEGIN  CR 3 SPACES  DUP .CONNECTION  @ ?DUP 0= UNTIL ;
: .CIRCUITS  CIRCUITS BEGIN  @ ?DUP WHILE  DUP .CIRCUIT  REPEAT ;
: C .CIRCUITS ;

\ To add a new connection, if either of the boxes is part of an existing
\ circuit, add it to that. Otherwise create a new circuit.

: ADD ( b1 b2 circuit -- )  CELL+ LINK,  , , ;
: NEW ( b1 b2 -- )  HERE  CIRCUITS LINK, 0 ,  ADD ;

: IN-CONNECTION ( b connection -- 0/1 )
    2DUP CELL+ @ =  -ROT 2 CELLS + @ = OR  NEGATE ;


: CONNECTS? ( b1 b2 connection -- f )  CONN@ 2>R ( b1 b2 )
    OVER R@ =  OVER R> = OR  SWAP R@ = OR  SWAP R> = OR ;

T{ CIRCUITS OFF  1 2 NEW -> }T
T{ 1 6 CIRCUITS @ CIRC@ CONNECTS? -> TRUE }T
T{ 2 6 CIRCUITS @ CIRC@ CONNECTS? -> TRUE }T
T{ 3 6 CIRCUITS @ CIRC@ CONNECTS? -> FALSE }T

: BOX-IN-CIRCUIT ( b circuit -- 0/1 )  CIRC@
    BEGIN  >R  2DUP R@ CONNECTS? IF  R> DROP  2DROP TRUE EXIT  THEN  R>
    @ ?DUP 0= UNTIL  2DROP FALSE ;


: IN-CIRCUIT? ( b1 b2 circuit -- f )  CIRC@
    BEGIN  >R  2DUP R@ CONNECTS? IF  R> DROP  2DROP TRUE EXIT  THEN  R>
    @ ?DUP 0= UNTIL  2DROP FALSE ;

T{ CIRCUITS OFF  1 2 NEW -> }T
T{ 1 6 CIRCUITS @ IN-CIRCUIT? -> TRUE }T
T{ 2 6 CIRCUITS @ IN-CIRCUIT? -> TRUE }T
T{ 3 6 CIRCUITS @ IN-CIRCUIT? -> FALSE }T

\ If one box is in an existing circuit, add to that one
\ If both boxes are in a circuit, do nothing
\ If neither box is in a circuit, add a new circuit
\ : CIRCUIT+ ( b1 b2 circuit -- )

: CONNECT ( b1 b2 -- )
    CIRCUITS BEGIN  @ ?DUP WHILE ( b1 b2 circuit )


        >R  2DUP R@ IN-CIRCUIT? IF  R> ADD EXIT  THEN  R>
    REPEAT  NEW ;
