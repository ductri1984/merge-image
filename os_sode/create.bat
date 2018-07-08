@echo off

set current="%cd%"
set path1=%current%\1.Requirement.And.Prototyping
set path2=%current%\2.Architecture.And.Database.Design
set path3=%current%\3.UXUI.And.API.Design
set path4=%current%\4.Database.Detail
set path5=%current%\5.1.API.Detail.And.Testing
set path6=%current%\5.2.Frontend.And.Testing
set path7=%current%\6.Acception.And.Feedback

set file1=%path1%\prototyping.txt
set file2=%path1%\requirement.txt

if not exist %path1% (
  mkdir %path1%
  echo '' >%file1%
  echo '' >%file2%
)
if not exist %path2% (
  mkdir %path2%
)
if not exist %path3% (
  mkdir %path3%
)
if not exist %path4% (
  mkdir %path4%
)
if not exist %path5% (
  mkdir %path5%
)
if not exist %path6% (
  mkdir %path6%
)
if not exist %path7% (
  mkdir %path7%
)

pause