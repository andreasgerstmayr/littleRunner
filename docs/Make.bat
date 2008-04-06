@ECHO OFF
REM Builds Sphinx documentation

REM You can set these variables from the command line.
SETLOCAL
set SPHINXOPTS=
set SPHINXBUILD=python ..\sphinx-build.py
set PAPER=
set PAPEROPT_a4=-D latex_paper_size=a4
set PAPEROPT_letter=-D latex_paper_size=letter
set ALLSPHINXOPTS= -d _build\doctrees %PAPEROPT_a4% %SPHINXOPTS%

REM ~ echo %SPHINXOPTS%
REM ~ %SPHINXBUILD%
REM ~ echo %ALLSPHINXOPTS%

@echo %0 %1
IF NOT "%1" == "" (GOTO target) ELSE (GOTO help)

:help
@echo Please use "builddocs target" where target is one of
@echo   html      to make standalone HTML files
@echo   web       to make files usable by Sphinx.web
@echo   htmlhelp  to make HTML files and a HTML help project
@echo   latex     to make LaTeX files, you can set PAPER=a4 or PAPER=letter
@echo   changes   to make an overview over all changed/added/deprecated items
@echo   linkcheck to check all external links for integrity
goto endb

@echo This line should never be reached
REM  .PHONY: help clean html web htmlhelp latex changes linkcheck



:target
IF "%1" == "clean" GOTO %1
IF "%1" == "html"  GOTO %1
IF "%1" == "web" GOTO %1
IF "%1" == "htmlhelp" GOTO %1
IF "%1" == "latex" GOTO %1
IF "%1" == "changes" GOTO %1
IF "%1" == "linkcheck" GOTO %1
REM If the argument isn't above, complain and show help
goto unrecognized

goto endb


:clean
@echo rmdir /s _build\*
goto endb

:html
mkdir  _build\html
mkdir _build\doctrees
%SPHINXBUILD% -b html %ALLSPHINXOPTS% _build\html
REM 	@echo
@echo "Build finished. The HTML pages are in _build\html."
goto endb


:pickle
mkdir _build\pickle
mkdir _build\doctrees
%SPHINXBUILD% -b pickle %ALLSPHINXOPTS% _build\pickle
REM 	@echo
@echo Build finished; now you can run python -m sphinx.web _build\pickle to start the server.
goto endb


:htmlhelp
mkdir _build\htmlhelp
mkdir _build\doctrees
%SPHINXBUILD% -b htmlhelp %ALLSPHINXOPTS% _build\htmlhelp
REM 	@echo
@echo "Build finished; now you can run HTML Help Workshop with the .hhp project file in _build\htmlhelp.
goto endb

:latex
mkdir _build\latex
mkdir _build\doctrees
%SPHINXBUILD% -b latex %ALLSPHINXOPTS% _build\latex
REM 	@echo
@echo Build finished; the LaTeX files are in _build\latex.
@echo Run \`make all-pdf' or \`make all-ps' in that directory to  run these through (pdf)latex.
goto endb

:changes
mkdir _build\changes
mkdir _build\doctrees
%SPHINXBUILD% -b changes %ALLSPHINXOPTS% _build\changes
REM 	@echo
echo "The overview file is in _build\changes."
goto endb

:linkcheck
mkdir _build\linkcheck
mkdir _build\doctrees
%SPHINXBUILD% -b linkcheck %ALLSPHINXOPTS% _build\linkcheck
REM 	@echo
@echo Link check complete; look for any errors in the above output or in _build\linkcheck\output.txt.
goto endb

:doctest
mkdir _build\doctest
mkdir _build\doctrees
%SPHINXBUILD% -b doctest %ALLSPHINXOPTS% _build\doctest
REM
goto endb

:unrecognized
echo Unrecognized option %1
goto help

:endb
ENDLOCAL