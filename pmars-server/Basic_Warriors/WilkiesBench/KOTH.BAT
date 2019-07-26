@echo off
rem Written by: Steve Bailey (sgb@zed-inst.demon.co.uk)
rem Launches fight.bat and then runs the process.bas cleanup program.
rem Modified by John K. Wilkinson (jwilkinson@mail.utexas.edu), 3-20-96.

if "%1"=="."  echo on
if "%1"=="."  shift

if "%1"==""  goto help
if "%2"==""  goto help

goto doit

:help
echo Usage "KOTH filename rounds"
echo    Filename is the name of the warrior you wish to
echo    test, _without_ the ".red" extension.
echo.
echo    Rounds is the number of rounds to fight, per warrior.
echo.
echo    When you are experienced with finding warriors to test
echo    your programs against, you can easily alter this batch
echo    file to your needs.  Until then, find out how many
echo    'Wilkies' your warrior gets! ;-)
goto :end

:doit
set eol=
rem eol is null, purely set to allow a defined number
rem of spaces at end of line.
set pre=pmars -r %2 %1.red %eol%
set post=.red
echo %1>koth_res.tmp
echo %pre%%1%post% >>koth_res.tmp
echo Enemy - - - w l t >>koth_res.tmp

echo Fighting paper-style warriors...
call fight time
call fight nobody
call fight paperone
call fight marcia13
echo Fighting stone-style warriors...
call fight bluefunk
call fight cannon
call fight tornado
call fight fstorm
echo Fighting scissor-style warriors...
call fight rave
call fight irongate
call fight pswing
call fight thermite

qbasic /run process.bas
del *.tmp
type %1.log
:end