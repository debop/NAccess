@echo off
@echo =========================================
@echo ��� ������Ʈ�� release ���� �����մϴ�.
@echo �׽�Ʈ Assembly�� �����Ϸ��� buildall ���� test �� �߰��ϼ���.
@echo =========================================
cd ..
nant cleanall quick obfuscator release-all-frameworks > Build.log