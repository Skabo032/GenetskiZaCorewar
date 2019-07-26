echo %1
%pre%%1%post% >fight.tmp
echo %1.red >>koth_res.tmp
find "Results" <fight.tmp >>koth_res.tmp
find "Results" <fight.tmp