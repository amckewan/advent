\ Day 8, part 1

include ../init.fs

0 VALUE BOXES
0 VALUE #BOXES

: BOX ( box# -- a )  3 CELLS * BOXES + ;

: X   ( box -- x )   @ ;
: Y   ( box -- y )   CELL + @ ;
: Z   ( box -- z )   2 CELLS + @ ;

: .BOX#   ." [" 0 .R ." ] " ;
: .BOX ( box# )  DUP .BOX#  BOX  DUP X 0 .R  ." ,"  DUP Y 0 .R  ." ,"  Z . ;

: DIST ( b1 b2 -- n ) \ sqrt((x2-x1)^2 + (y2-y1)^2 + (z2-z1)^2)
    OVER BOX X  OVER BOX X -  DUP *  >R
    OVER BOX Y  OVER BOX Y -  DUP *  >R
         BOX Z  SWAP BOX Z -  DUP *  2R> + +
    S>F FSQRT 100e F* F>S ;

0 VALUE DISTANCES
: 'DIST ( b1 b2 -- a )  #BOXES * +  CELLS DISTANCES + ;
: PRECALC ( -- ) \ pre-calculate distances
    HERE TO DISTANCES  #BOXES DUP * CELLS ALLOT
    #BOXES 0 DO  #BOXES 0 DO  I J DIST  I J 'DIST !  LOOP LOOP ;
: DISTANCE  ( b1 b2 -- n )  'DIST @ ;

: .DISTANCES
    CR 3 SPACES  #BOXES 0 DO I 5 .R LOOP
    #BOXES 0 DO  CR I 2 .R ." : "
      I 1+ 0 DO  I J DISTANCE 5 .R  LOOP LOOP ;

: :BOXES  ALIGN  HERE TO BOXES ;
: ;BOXES  HERE BOXES - 3 CELLS / TO #BOXES  PRECALC ;

: EXAMPLE
    :BOXES
(  0 )  162  ,  817  ,  812 , 
(  1 )  57  ,  618  ,  57 , 
(  2 )  906  ,  360  ,  560 , 
(  3 )  592  ,  479  ,  940 , 
(  4 )  352  ,  342  ,  300 , 
(  5 )  466  ,  668  ,  158 , 
(  6 )  542  ,  29  ,  236 , 
(  7 )  431  ,  825  ,  988 , 
(  8 )  739 , 650 , 466 , 
(  9 )  52 , 470 , 668 , 
( 10 )  216 , 146 , 977 , 
( 11 )  819 , 987 , 18 , 
( 12 )  117 , 168 , 530 , 
( 13 )  805 , 96 , 715 , 
( 14 )  346 , 949 , 466 , 
( 15 )  970 , 615 , 88 , 
( 16 )  941 , 993 , 340 , 
( 17 )  862 , 61 , 35 , 
( 18 )  984 , 92 , 344 , 
( 19 )  425 , 690 , 689 , 
    ;BOXES ;
EXAMPLE
T{ 1 3 DISTANCE -> 1 3 DIST }T

: INPUT  :BOXES  S" input.fs" INCLUDED  ;BOXES ;


\ find the pair of boxes with the shortest distance, but above floor
: SHORTEST ( floor -- b1 b2 )   0 0 ROT 1+ -1 ( b1 b2 floor+1 shortest )
    #BOXES 0 DO  #BOXES 0 DO ( 1,000,000 times )
        2DUP I J DISTANCE -ROT WITHIN ( > floor and < shortest )
        IF  DROP NIP NIP  I J  ROT  I J DISTANCE  THEN
    LOOP LOOP 2DROP ;


\ A circuit is an array of #BOXES bytes, 1=present in circuit
VARIABLE CIRCUITS
: LINK ( a -- )  HERE  OVER @ ,  SWAP ! ;
: NEW-CIRCUIT ( -- circ )  HERE  CIRCUITS LINK  HERE #BOXES DUP ALLOT ERASE ;
: HAS ( box circ -- f )  CELL+ +  C@ ;
: ADD ( box circ -- )    CELL+ +  1 SWAP C! ;
: ADD-CONNECTION ( b1 b2 circ -- )  2DUP ADD  NIP ADD ;

: SIZE ( circ -- n )  0 SWAP  CELL+ #BOXES BOUNDS DO  I C@ +  LOOP ;

: .() ( n )  ."   (" 0 .R ." ) " ;
: .CIRCUIT ( circ -- )  #BOXES 0 DO  I OVER HAS IF I . THEN  LOOP DROP ;
: .CIRCUITS   CIRCUITS BEGIN @ ?DUP WHILE
    CR  DUP SIZE .()  DUP .CIRCUIT   REPEAT ;

\ When we add a new connection, there are several possibilities:
\ 1. Both boxes are already in a circuit, do nothing
\ 2. One of the boxes is in a circuit, add the other box
\ 3. The boxes are in two difference circuits, join the circuits
\ 4. Neither box is in a circuit, add a new circuit

: FIND-CIRCUIT ( box -- circ )
    CIRCUITS BEGIN @ DUP WHILE
        2DUP HAS IF  NIP EXIT  THEN
    REPEAT  NIP ;

: REMOVE ( circ -- ) \ CELL+ #BOXES ERASE EXIT
    CIRCUITS BEGIN  2DUP @ = IF  SWAP @ SWAP !  EXIT THEN  @ AGAIN ;

: JOIN ( circ1 circ2 -- )
    #BOXES 0 DO  OVER I HAS IF  I OVER ADD  THEN  LOOP
    DROP REMOVE ;

: CONNECT ( b1 b2 -- )
    OVER FIND-CIRCUIT ?DUP IF ( b1 in a circuit )
        OVER FIND-CIRCUIT ?DUP IF ( b2 in a circuit )
            2DUP = IF ( same circuit ) 2DROP  ELSE  JOIN  THEN
            2DROP EXIT
        THEN ( b2 not in a circuit ) ADD DROP EXIT
    THEN ( b1 not in a circuit )
    DUP FIND-CIRCUIT ?DUP IF  NIP ADD EXIT  THEN
    NEW-CIRCUIT ADD-CONNECTION ;

: MAKE-CIRCUITS ( n )  CIRCUITS OFF
    0 ( floor)  SWAP 0 DO  SHORTEST  2DUP CONNECT  DISTANCE  LOOP  DROP ;

: C CONNECT ;
: L .CIRCUITS ;
: M MAKE-CIRCUITS ;

10 MAKE-CIRCUITS

( PART 2 )
1 VALUE FLOOR
2VARIABLE LAST
: NEXT ( -- b1 b2 )  FLOOR SHORTEST  2DUP DISTANCE 1+ TO FLOOR  2DUP LAST 2! ;
: COMPLETE? ( -- f )  CIRCUITS @ SIZE #BOXES = ;
: ANSWER ( -- n )  LAST 2@  BOX X  SWAP  BOX X * ;
: PART2 ( -- n ) 1 TO FLOOR  BEGIN  NEXT CONNECT  COMPLETE? UNTIL  ANSWER ;

T{ EXAMPLE  10 MAKE-CIRCUITS  PART2 -> 25272 }T
\ input -> 170629052
