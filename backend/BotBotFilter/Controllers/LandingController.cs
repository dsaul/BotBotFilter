using DanSaul.SharedCode.CardDav;
using DanSaul.SharedCode.CardDAV;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using Serilog;

namespace TwilioCallScreening.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class LandingController : TwilioController
	{

		InMemoryCardDavController InMemoryCardDavController { get; init; }


		public LandingController(
			InMemoryCardDavController _InMemoryCardDavController
			)
		{
			InMemoryCardDavController = _InMemoryCardDavController;

		}


		[HttpPost("Entry")]
		public TwiMLResult Entry()
		{
			VoiceResponse response = new();

			HashSet<VCard> cards = InMemoryCardDavController.VCards;

			string? from = Request.Form["From"];
			if (string.IsNullOrWhiteSpace(from))
			{
				Log.Warning("Rejecting Call with no From");
				response.Reject();
				goto end;
			}

			foreach (VCard card in cards)
			{
				foreach (VCardAttributeTelephone telephoneAttr in card.TelephoneNumbers)
				{
					if (telephoneAttr.E164 == from)
					{
						string e164 = Env.FORWARD_E164;
						response.Dial(e164);
						goto end;
					}
				}
			}


			Random rnd = new();
			string randomTarget = rnd.Next(1, 9).ToString();

			Gather gather = new(action: new Uri($"EntryCaptcha/{randomTarget}", UriKind.Relative), numDigits: 1);
			
			gather.Play(new Uri(Env.SND_URI_INTRO));
			gather.Play(new Uri(string.Format(Env.SND_URI_NUMBER_PATTERN, randomTarget)));
			gather.Pause(5);
			gather.Play(new Uri(Env.SND_URI_INTRO));
			gather.Play(new Uri(string.Format(Env.SND_URI_NUMBER_PATTERN, randomTarget)));
			gather.Pause(5);
			gather.Play(new Uri(Env.SND_URI_INTRO));
			gather.Play(new Uri(string.Format(Env.SND_URI_NUMBER_PATTERN, randomTarget)));
			gather.Pause(5);
			
			response.Append(gather);

			response.Play(new Uri(Env.SND_URI_NO_RESPONSE_GOOD_BYE));

			response.Hangup();


			end:

			return TwiML(response);
		}

		[HttpPost("EntryCaptcha/{target}")]
		public TwiMLResult EntryCaptcha(
			[FromRoute] string? target, 
			[FromForm] string? digits
			)
		{
			VoiceResponse response = new();

			if ($"{digits ?? ""}".Trim() != $"{target ?? ""}".Trim())
			{
				response.Play(new Uri(Env.SND_URI_NOT_CORRECT_GOOD_BYE));

				response.Hangup();
			}
			else
			{
				response.Dial(Env.FORWARD_E164);
			}

			return TwiML(response);
		}
	}
}
