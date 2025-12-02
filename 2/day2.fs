\ Day 2, part 1

[DEFINED] EMPTY [IF] EMPTY [THEN] MARKER EMPTY

include input.fs ( defined INPUT )
: EXAMPLE S" 11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124" ;

\ Check if ID is invalid
: INVALID? ( n -- f )  >R ( n)  11 ( divisor) 100 ( limiter)
    BEGIN  DUP 1000000000000000000 <= WHILE
        R@ OVER < IF ( even # digits)  DROP   R> SWAP MOD 0=  EXIT  THEN
        10 *  R@ OVER < IF ( odd #)    2DROP  R> DROP FALSE   EXIT  THEN
        10 *     ( limiter)  SWAP
        10 * 9 - ( divisor)  SWAP
    REPEAT   2DROP  R> DROP  FALSE ;

VARIABLE INVALID  ( sum of invalid IDs )

: CHECK-RANGE ( from to -- )
    1+ SWAP DO  I INVALID? IF  I INVALID +!  THEN  LOOP ;

: NUMBER ( a n -- u a+ n- )  0. 2SWAP >NUMBER  ROT DROP ;
: ?DELIM ( a n c -- a' n' )  >R OVER C@ R> <> ABORT" delim?"  1 /STRING ;

: CHECK ( a n -- )  0 INVALID !
    BEGIN   NUMBER  '-' ?DELIM  NUMBER  2>R  CHECK-RANGE  2R>
        DUP WHILE   ',' ?DELIM 
    REPEAT  2DROP ;

INPUT CHECK
CR INVALID ?


( ===== TESTS ===== )
include ../ttester.fs

T{ EXAMPLE CHECK  INVALID @ -> 1227775554 }T
T{ INPUT CHECK    INVALID @ -> 38158151648 }T

T{ -> }T
T{ 11           INVALID? -> TRUE }T
T{ 22           INVALID? -> TRUE }T
T{ 99           INVALID? -> TRUE }T
T{ 1010         INVALID? -> TRUE }T
T{ 1188511885   INVALID? -> TRUE }T
T{ 222222       INVALID? -> TRUE }T
T{ 446446       INVALID? -> TRUE }T
T{ 38593859     INVALID? -> TRUE }T

T{ 0            INVALID? -> TRUE  }T ( quirk )
T{ 97           INVALID? -> FALSE }T
T{ 1231234      INVALID? -> FALSE }T
T{ 100101       INVALID? -> FALSE }T


