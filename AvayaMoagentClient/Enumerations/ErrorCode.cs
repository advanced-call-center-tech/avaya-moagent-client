//Copyright (c) 2010 - 2012, Matthew J Little and contributors.
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are permitted
//provided that the following conditions are met:
//
//  Redistributions of source code must retain the above copyright notice, this list of conditions
//  and the following disclaimer.
//
//  Redistributions in binary form must reproduce the above copyright notice, this list of
//  conditions and the following disclaimer in the documentation and/or other materials provided
//  with the distribution.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
//IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
//DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
//WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
//ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvayaMoagentClient.Enumerations
{
  /// <summary>
  /// ErrorCode
  /// </summary>
  public static class ErrorCode
  {
    public const string CallCancelNotPermitted = "E28429";
    public const string TransferFailedTryAgainLater = "E28628";
    public const string JobNotRunning = "E28804";
    public const string JobNotReadyLogOnLater = "E28805";
    public const string AlreadyLoggedIn = "E28812";
    public const string MaximumAgentLimitReached = "E28813";
    public const string ManagedAgentsNotPermittedOnJob = "E28814";
    public const string SalesVerificationWithUnitWorkListsNotPermitted = "E28815";
    public const string InboundAgentsOnlyPermitted = "E28816";
    public const string OutboundAgentsOnlyPermitted = "E28817";
    public const string OutboundOrManagedAgentsOnlyPermitted = "E28818";
    public const string OnlyOutboundAgentsPermittedBySalesVerification = "E28819";
    public const string CannotOpenChannelToOpMonProcess = "E28850";
    public const string TelephoneLineNotAvailable = "E28866";
    public const string TelephoneLineNotOffhook = "E28867";
    public const string InvalidHeadsetId = "E28871";
    public const string HeadsetIdNotReservedNorValidated = "E28873";
    public const string NoHeadsetConnectRequestPending = "E28875";
    public const string HeadsetNotConnected = "E28876";
    public const string HeadsetNotDisconnected = "E28878";
    public const string HeadsetConnectionBroken = "E28880";
    public const string HeadsetConnectionReconnected = "E28881";
    public const string AlreadyAvailableCannotChangeClass = "E28882";
    public const string InvalidWorkClass = "E28883";
    public const string NotAttachedToJob = "E28885";
    public const string AttachedToJobAlreadyMustDetach = "E28889";
    public const string FailureToOpenJobResourceFile = "E28890";
    public const string FieldNameNotFound = "E28894";
    public const string HeadsetMustBeActive = "E28896";
    public const string JobNotAvailable = "E28898";
    public const string NoAvailableForWorkRequestPending = "E28899";
    public const string WrongMessageIdReceived = "E28900";
    public const string JobAttachedMustDetachFirst = "E28916";
    public const string NoJobAttached = "E28917";
    public const string NotAvailableForWorkOnJob = "E28918";
    public const string HeadsetIdNotFoundInResevedList = "E28920";
    public const string KickedFromDialer = "E28921";
    public const string NoReserveHeadsetRequestPending = "E28922";
    public const string HeadsetIdAlreadyReserved = "E28923";
    public const string AlreadySignedOn = "E28925";
    public const string InvalidCredentials = "E28926";
    public const string AgentNotAcquiredYet = "E28946";
    public const string AgentNotAllowedToLogoff = "E28967";
    public const string ManagedDialingCapableJobsOnly = "E29000";
    public const string FailedToAttachUnitSegment = "E29206";
    public const string FeatureNotAvailableInSoftdialerMode = "E29950";
    public const string FailedToJoinJob = "E29952";
    public const string ExceededNumberOfAgents = "E50100";
    public const string NoMoreHeadsetsPermitted = "E50612";
    public const string FailureToAccessHeadsetTable = "E50613";
    public const string RingbackTimeout = "E70002";
    public const string MustSelectUnit = "E70000";
    public const string PasswordExpired = "E70012";
  }
}
