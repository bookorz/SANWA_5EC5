ABT_S1:'S1F0'
<B[10] 0x0 0x0 0x81 0xF 0x0 0x0 0xE 0xAC 0xE7 0xAE>
.
Are You There Request(R):'S1F1' W
.

On Line Data (D):'S1F2' 
<L[2]
	<A[1] 'MDLN'>
	<A[1] 'SOFTREV'>
>
.
Selected Equipment Status Request (SSR):'S1F3' W
<L[0]
	<A[0] 'SVID'>
>
.
Selected Equipment Status Data (SSD):'S1F4' 
<L[0]
	<A[0] 'SV'> 
.
Status Variable Namelist Request (SVNR):'S1F11' W
<L[0]
	<A[0] 'SVID'>
>
.
Status Variable Namelist Reply (SVNRR):'S1F12'
<L[0]
	<L[3]
		<A[0] 'SVID'>
		<A[0] 'SVNAME'>
		<A[0] 'UNITS'>
	>
>
.
Establish Communications Request (CR):'S1F13' W
<L[2]
    <A[1] 'MDLN'>
    <A[1] 'SOFTREV'>
>
.
Establish Communications Request Acknowledge (CRA):'S1F14' 
<L[2]
    <A[0] 'COMMACK'>
	<L[2]
		<A[1] 'MDLN'>
		<A[1] 'SOFTREV'>
	>
>
.
Request OFF-LINE (ROFL):'S1F15' W
.
OFF-LINE Acknowledge:'S1F16'
<A[0] 'OFLACK'>
.
Request ON-LINE (RONL):'S1F17' W
.
ON-LINE Acknowledge (ONLA):'S1F18'
<A[0] 'ONLACK'>
.
ABT_S2:'S2F0' 
<B[10] 0x0 0x0 0x82 0xD 0x0 0x0 0x47 0xE 0x5D 0x87>	
.
Equipment Constant Request (ECR):'S2F13' W
<L[0]
	<A[0] 'ECID'>
>
.
Equipment Constant Data (ECD):'S2F14' 
<L[0]
	<A[0] 'ECV'>
>
.
New Equipment Constant Send (ECS) :'S2F15' W
<L[0]
    <L[2]
		<A[0] 'ECID'>
		<A[0] 'ECV'>
	>
>
.
New Equipment Constant Acknowledge (ECA) :'S2F16' 
<A[0] 'EAC'>
.
Date and Time Request (DTR):'S2F17' W
.
Date and Time Data (DTD):'S2F18' 
<A[0] 'TIME'>
.
Trace Initialize Send (TIS):'S2F23' W
<L[5]
    <A[1]'TRID'>
    <A[1]'DSPER'>
    <A[1]'TOTSMP'>
    <A[1]'REPGSZ'>
    <L[0]
        <A[1]'SVID'>
    >
>
.
Trace Initialize Acknowledge (TIA):'S2F24' 
<A[0] 'TIAACK'>
.
Equipment Constant Namelist Request (ECNR):'S2F29' W
<L[0]
	<A[1]'ECID'>
>
.
Equipment Constant Namelist (ECN) :'S2F30' 
<L[0]
    <L[6]
        <A[1] 'ECID'>
        <A[1] 'ECNAME'>
        <A[1] 'ECMIN'>
        <A[1] 'ECMAX'>
        <A[1] 'ECDEF'>
        <A[1] 'UNITS'>
    >
>
.
Date and Time Set Request (DTS):'S2F31' W
<A[0] 'TIME'>
.
Date and Time Set Acknowledge (DTA):'S2F32' 
<A[0] 'TIACK'>
.
Define Report (DR):'S2F33' W
<L[0]
	<A[0] 'DATAID'>
	<L[2]
		<A[0] 'RPTID'>
		<L[0]
			<A[0]'VID'>
		>
	>
>
.
Define Report Acknowledge (DRA):'S2F34'
<A[0] 'DRACK'>
.
Link Event Report (LER):'S2F35' W
<L[0]
	<A[0] 'DATAID'>
	<L[2]
		<A[0] 'CEID'>
		<L[0]
			<A[0]'PRTID'>
		>
	>
>
.
Link Event Report Acknowledge (LERA):'S2F36'
<A[0] 'LRACK'>
.
Enable/Disable Event Report (EDER):'S2F37' W 
<L[2]
	<A[0] 'CEED'>
	<L[0]
		<A[0] 'CEID'>
	>
>
.
Enable/Disable Event Report Acknowledge (EERA):'S2F38'
<A[0] 'ERACK'>
.
//Replay by AP
Host Command Send (HCS) :'S2F41' W
<L[2]
    <A[1] 'RCMD'>
    <L[0]
       <L[2]
            <A[0] 'CPNAME'>
            <A[1] 'CPVAL'>
        >
    >
>
.
//Replay by AP
Host Command Acknowledge (HCA):'S2F42' 
<L[2]
    <A[0] 'HCACK'>
    <L[2]
		<A[0] 'CPNAME'>
		<A[0] 'CPVAL'>
    >
>
.
ABT_S3:'S3F0'
<B[10] 0x0 0x0 0x83 0x11 0x0 0x0 0x71 0x11 0x86 0xAC>
.
Carrier Action Request:'S3F17' W
<L[5]
    <A[0] 'DATAID'>
	<A[0] 'CARRIERCTION'>
	<A[0] 'CARRIERID'>
	<A[0] 'PTN'>
	<L[0]
		<A[1] 'CATTRID'>
		<A[1] 'CATTRDATA'>
	>		
>
.
Carrier Action Acknowledge:'S3F18'
<L[2]
    <A[0] 'CAACK'>
	<L[2]
		<A[0] 'EERCODE'>
		<A[0] 'EERTEXT'>
	>
>
.
Cancel All Carrier Out Request:'S3F19' W
.
Cancel All Carrier Out Acknowledge:'S3F20'
<L[2]
    <A[0] 'CAACK'>
	<L[2]
		<A[0] 'EERCODE'>
		<A[0] 'EERTEXT'>
	>
>
.
Port Group Definition:'S3F21' W
<L[3]
	<A[0] 'PORTGRPNAME'>
	<A[0] 'ACCESSMODE'>
	<L[0]
		<A[0] 'PTN'>		
	>
>
.
Port Group Definition Acknowledge:'S3F22'
<L[2]
    <A[0] 'CAACK'>
	<L[2]
		<A[0] 'EERCODE'>
		<A[0] 'EERTEXT'>
	>
>
.
Port Group Action Request:'S3F23' W
<L[3]
    <A[0] 'PGRPACTION'>
    <A[0] 'PORTGRPNAME'>
    <L[0]
        <L[2]
            <A[1] 'PARAMNAME'>
            <A[1] 'PARAMVAL'>
        >
    >
>
.
Port Group Action Acknowledge:'S3F24' W
<L[2]
    <A[0] 'CAACK'>
    <L[0]
        <L[2]
            <A[1] 'ERRCODE'>
            <A[1] 'ERRTEXT'>
        >
    >
>
.
Port Action Request:'S3F25' W
<L[3]
    <A[0] 'PORTACTION'>
    <A[0] 'PTN'>
    <L[0]
        <L[2]
            <A[1] 'PARAMNAME'>
            <A[1] 'PARAMVAL'>
        >
    >
>
.
PortActionAcknowledge :'S3F26'  
<L[2]
    <A[0] 'CAACK'>
    <L[0]
        <L[2]
            <A[1] 'ERRCODE'>
            <A[1] 'ERRTEXT'>
        >
    >
>
.
Change Access:'S3F27' W
<L[2]
    <A[0] 'ACCESSMODE'>
	<L[0]
		<A[0] 'PTN'>
	>
>
.
Change Access Acknowledge:'S3F28'
<L[2]
    <A[0] 'CAACK'>
    <L[0]
        <L[3]
            <A[1] 'PTN'>
            <A[1] 'ERRCODE'>
            <A[1] 'ERRTEXT'>
        >
    >
>
.
ABT_S5:'S5F0' 
<B[10] 0x0 0x0 0x85 0x3 0x0 0x0 0x47 0xE 0x5D 0x89>	
.
Enable/Disable Alarm Send (EAS):'S5F3' W
<L[2]
    <A[0] 'ALED'>
    <A[0] 'ALID'>
>
.
Enable/Disable Alarm Acknowledge (EAA) :'S5F4' 
<A[1] 'ACKC5'>
.
List Alarms Request (LAR):'S5F5' W
<A[0] 'ALID'>
.
List Alarm Data (LAD):'S5F6' 
<L[0]
    <L[3]
        <A[1] 'ALCD'>
        <A[1] 'ALID'>
        <A[1] 'ALTX'>
    >
>
.
ABT_S6:'S6F0' 
.
Event Report Send (ERS) :'S6F11' W 
<L[3]
    <A[1] 'DATAID'>
    <A[1] 'CEID'>
    <L[0]
        <L[2]
            <A[1] 'RPTID'>
            <L[0]
				<A[1] 'V'>
			>
    >
>
.
Event Report Acknowledge (ERA) :'S6F12' 
<A[1] 'ACKC6'>
.
Annotated Event Report Send (AERS) :'S6F13' W 
<L[3]
    <A[1] 'DATAID'>
    <A[1] 'CEID'>
    <L[0]
        <L[2]
            <A[1] 'RPTID'>
            <L[0]
				<L[2]
					<A[1] 'VID'>
					<A[1] 'V'>
				>
			>
    >
>
.
Annotated Event Report Acknowledge (AERA)  :'S6F14' 
<A[1] 'ACKC6'>
.
Event Report Request (ERR):'S6F15' W
<A[1] 'CEID'>
.
S6,F16  Event Report Data (ERD):'S6F16' 
<L[3]
    <A[1] 'DATAID'>
    <A[1] 'CEID'>
    <L[0]
        <L[2]
            <A[1] 'RPTID'>
            <L[0]
				<A[1] 'V'>
			>
    >
>
.
Annotated Event Report Request (AERR):'S6F17' W
<A[1] 'CEID'>
.
Annotated Event Report Data (AERD):'S6F18' 
<L[3]
    <A[1] 'DATAID'>
    <A[1] 'CEID'>
    <L[0]
        <L[2]
            <A[1] 'RPTID'>
            <L[0]
				<L[2]
					<A[1] 'VID'>
					<A[1] 'V'>
				>
			>
    >
>
.
Individual Report Request (IRR):'S6F19' W
<A[1] 'RPTID'>
.
Individual Report Data (IRD):'S6F20' 
<L[0]
    <A[1] 'V'>
>
.
Annotated Individual Report Request (AIRR):'S6F21' W
<A[1] 'RPTID'>
.
Annotated Individual Report Data (AIRD):'S6F22' 
<L[0]
    <L[2]
        <A[1] 'VID'>
        <A[1] 'V'>
    >
>
.
//Replay by AP
Unrecognized Device ID (UDN):'S9F1'
<A[1] 'MHEAD'> 
.
//Replay by AP
Unrecognized Stream Type (USN) :'S9F3'
<A[1] 'MHEAD'> 
.
//Replay by AP
Unrecognized Function Type (UFN):'S9F5'
<A[1] 'MHEAD'> 
.
//Replay by AP
Illegal Data (IDN) :'S9F7'
<A[1] 'MHEAD'> 
.
ABT_S10:'S10F0' 
	<B[10] 0x0 0x0 0x8A 0x3 0x0 0x0 0x47 0xE 0x5D 0x8B>	
.
Terminal Request (TRN):'S10F1' W
<L[2]
    <A[1] 'TID'>
    <A[1] 'TEXT'>
>
.
Terminal Request Acknowledge (TRA) :'S10F2' 
<A[1] 'ACKC10'>
.
Terminal Display, Single (VTN):'S10F3' W
<L[2]
    <A[1] 'TID'>
    <A[1] 'TEXT'>
>
.
Terminal Display, Single Acknowledge (VTA):'S10F4' 
<A[1] 'ACKC10'>
.
