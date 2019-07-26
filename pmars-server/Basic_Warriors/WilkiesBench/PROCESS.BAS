REM Processing file, computes scores, and creates an
REM at-a-glance score chart for a set of battles.
REM Written by: Steve Bailey  (sgb@zed-inst.demon.co.uk)
REM Modified by John K Wilkinson (jwilkinson@mail.utexas.edu), 3-20-96.

OPEN "koth_res.tmp" FOR INPUT AS #1
LINE INPUT #1, i$
OPEN i$ + ".log" FOR OUTPUT AS #2
PRINT #2, i$
LINE INPUT #1, i$
PRINT #2, i$
LINE INPUT #1, i$
PRINT #2, "- - - - - -  W   L   T   Score"
wt = 0
lt = 0
tt = 0
DO UNTIL EOF(1)
        LINE INPUT #1, o$
        o$ = LEFT$(o$ + "        ", 9) + ":"
       
        LINE INPUT #1, i$
        i$ = i$ + " "

        s% = INSTR(i$, ":")
        i$ = LTRIM$(MID$(i$, s% + 1))
       
        s% = INSTR(i$, " ")
        w = VAL(LEFT$(i$, s%))
        o$ = o$ + " " + RIGHT$("  " + STR$(w), 3)
        wt = wt + w
        i$ = MID$(i$, s% + 1)
       
        s% = INSTR(i$, " ")
        l = VAL(LEFT$(i$, s%))
        o$ = o$ + " " + RIGHT$("  " + STR$(l), 3)
        lt = lt + l
        i$ = MID$(i$, s% + 1)

        s% = INSTR(i$, " ")
        t = VAL(LEFT$(i$, s%))
        o$ = o$ + " " + RIGHT$("  " + STR$(t), 3)
        tt = tt + t
        ro = (300 * w / (w + l + t)) + (100 * t / (w + l + t))
        IF (ro < 10) THEN o$ = o$ + "      " + LEFT$(STR$(ro), 2): GOTO printit
        IF (ro < 100) THEN o$ = o$ + "     " + LEFT$(STR$(ro), 3): GOTO printit
        o$ = o$ + "    " + LEFT$(STR$(ro), 4)

printit:
        PRINT #2, o$
       
LOOP
PRINT #2, "- - - - - - - - - - - - - - -"
PRINT #2, "Total wins   = "; wt
PRINT #2, "Total losses = "; lt
PRINT #2, "Total ties   = "; tt
os = (300 * wt / (wt + lt + tt)) + (100 * tt / (wt + lt + tt))
PRINT #2, "Overall score is "; os

ii = 4
DO UNTIL ii = 0
i = 10
DO UNTIL i = 20
SOUND (40 + i * 100), (1)
i = i + 1
LOOP
ii = ii - 1
LOOP

CLOSE
SYSTEM

