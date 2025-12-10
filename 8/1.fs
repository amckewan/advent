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
    #BOXES 0 DO  #BOXES 0 DO
        2DUP I J DISTANCE -ROT WITHIN ( > floor and < shortest )
        IF  DROP NIP NIP  I J  ROT  I J DISTANCE  THEN
    LOOP LOOP 2DROP ;

\ Connection:   link, b1, b2
\ Circuit:      link, connection
VARIABLE CIRCUITS
: LINK, ( a -- )  HERE  OVER @ ,  SWAP ! ;

: ADD ( b1 b2 circuit -- )  CELL+ LINK, , , ;

: .() ( n )  ."  (" 0 .R ." ) " ;

: .CONNECTION ( a )  CELL+ 2@  2DUP .BOX ." - " .BOX  DISTANCE .() ;
: .CIRCUIT ( a )  BEGIN  CR DUP .CONNECTION  @ ?DUP 0= UNTIL ;
: .CIRCUITS  CIRCUITS BEGIN  @ ?DUP WHILE  DUP CELL+ @ .CIRCUIT  REPEAT ;


\  : IN-CIRCUIT? ( b circuit -- f )


\  : CIRCUIT, ( b1 b2 -- )  CIRCUITS LINK, , , ;

: CONNECT ( b1 b2 -- )
    CIRCUITS @ 0= IF  CIRCUITS LINK,  0 ,  THEN
    CIRCUITS @ ADD ;

